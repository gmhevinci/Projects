using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MotionFramework;
using MotionFramework.Console;
using MotionFramework.Resource;
using MotionFramework.Event;
using MotionFramework.Config;
using MotionFramework.Audio;
using MotionFramework.Network;
using MotionFramework.Patch;
using MotionFramework.Scene;
using MotionFramework.Pool;

public class GameLauncher : MonoBehaviour
{
	private class WebPost
	{
		public string AppVersion; //应用程序内置版本
		public int ServerID; //最近登录的服务器ID
		public int ChannelID; //渠道ID
		public string DeviceUID; //设备唯一ID
		public int TestFlag; //测试标记
	}
	public enum EQuality
	{
		Default,
		HD,
	}
	public enum ELanguage
	{
		Default,
		EN,
		KR,
	}

	[Tooltip("在编辑器下模拟运行")]
	public bool SimulationOnEditor = false;

	[Tooltip("是否跳过CDN服务器")]
	public bool SkipCDN = false;

	public EQuality Quality = EQuality.Default;
	public ELanguage Language = ELanguage.Default;


	void Awake()
	{
#if !UNITY_EDITOR
		SimulationOnEditor = false;
#endif

		if (SimulationOnEditor)
			SkipCDN = true;

		// 初始化应用
		InitAppliaction();

		// 初始化控制台
		if (Application.isEditor || Debug.isDebugBuild)
			DeveloperConsole.Initialize();

		// 初始化框架
		MotionEngine.Initialize(this, HandleMotionFrameworkLog);
	}
	void Start()
	{
		// 创建游戏模块
		StartCoroutine(CreateGameModules());
	}
	void Update()
	{
		// 更新框架
		MotionEngine.Update();
	}
	void OnGUI()
	{
		// 绘制控制台
		if (Application.isEditor || Debug.isDebugBuild)
			DeveloperConsole.Draw();
	}

	/// <summary>
	/// 初始化应用
	/// </summary>
	private void InitAppliaction()
	{
		Application.runInBackground = true;
		Application.backgroundLoadingPriority = ThreadPriority.High;

		// 设置最大帧数
		Application.targetFrameRate = 60;

		// 屏幕不休眠
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	/// <summary>
	/// 监听框架日志
	/// </summary>
	private void HandleMotionFrameworkLog(ELogLevel logLevel, string log)
	{
		if (logLevel == ELogLevel.Log)
		{
			UnityEngine.Debug.Log(log);
		}
		else if (logLevel == ELogLevel.Error)
		{
			UnityEngine.Debug.LogError(log);
		}
		else if (logLevel == ELogLevel.Warning)
		{
			UnityEngine.Debug.LogWarning(log);
		}
		else if (logLevel == ELogLevel.Exception)
		{
			UnityEngine.Debug.LogError(log);
		}
		else
		{
			throw new NotImplementedException($"{logLevel}");
		}
	}

	/// <summary>
	/// 创建游戏模块
	/// </summary>
	private IEnumerator CreateGameModules()
	{
		// 创建事件管理器
		MotionEngine.CreateModule<EventManager>();

		// 创建变体规则集合
		var variantRules = new List<VariantRule>();
		{
			var rule1 = new VariantRule();
			rule1.VariantGroup = new List<string>() { "EN", "KR" };
			rule1.TargetVariant = Language == ELanguage.Default ? VariantRule.DefaultTag : Language.ToString();
			variantRules.Add(rule1);

			var rule2 = new VariantRule();
			rule2.VariantGroup = new List<string>() { "HD" };
			rule2.TargetVariant = Quality == EQuality.Default ? VariantRule.DefaultTag : Quality.ToString();
			variantRules.Add(rule2);
		}

		// 资源服务接口
		IBundleServices bundleServices;
		if (SkipCDN)
		{
			var localBundleServices = new LocalBundleServices(variantRules);
			yield return localBundleServices.InitializeAsync(SimulationOnEditor);
			bundleServices = localBundleServices;
		}
		else
		{
			// 远程服务器信息
			string webServerIP = "http://127.0.0.1";
			string cdnServerIP = "http://127.0.0.1";
			string defaultWebServer = $"{webServerIP}/WEB/PC/GameVersion.php";
			string defaultCDNServer = $"{cdnServerIP}/CDN/PC";
			RemoteServerInfo serverInfo = new RemoteServerInfo(defaultWebServer, defaultCDNServer);
			serverInfo.AddServerInfo(RuntimePlatform.Android, $"{webServerIP}/WEB/Android/GameVersion.php", $"{cdnServerIP}/CDN/Android", $"{cdnServerIP}/CDN/Android");
			serverInfo.AddServerInfo(RuntimePlatform.IPhonePlayer, $"{webServerIP}/WEB/Iphone/GameVersion.php", $"{cdnServerIP}/CDN/Iphone", $"{cdnServerIP}/CDN/Iphone");

			// 向WEB服务器投递的数据
			WebPost post = new WebPost
			{
				AppVersion = Application.version,
				ServerID = PlayerPrefs.GetInt("SERVER_ID_KEY", 0),
				ChannelID = 0,
				DeviceUID = string.Empty,
				TestFlag = PlayerPrefs.GetInt("TEST_FLAG_KEY", 0)
			};

			var patchCreateParam = new PatchManager.CreateParameters();
			patchCreateParam.WebPoseContent = JsonUtility.ToJson(post);
			patchCreateParam.VerifyLevel = EVerifyLevel.Size;
			patchCreateParam.ServerInfo = serverInfo;
			patchCreateParam.VariantRules = variantRules;
			patchCreateParam.AutoDownloadDLC = new string[] { "panel" };
			patchCreateParam.AutoDownloadBuildinDLC = true;
			patchCreateParam.MaxNumberOnLoad = 4;

			PatchManager patchManager = MotionEngine.CreateModule<PatchManager>(patchCreateParam);
			yield return patchManager.InitializeAsync();
			bundleServices = MotionEngine.GetModule<PatchManager>();

			EventManager.Instance.AddListener<PatchEventMessageDefine.PatchStatesChange>(OnHandleEventMessage);
			EventManager.Instance.AddListener<PatchEventMessageDefine.OperationEvent>(OnHandleEventMessage);
		}

		// 创建资源管理器
		var resourceCreateParam = new ResourceManager.CreateParameters();
		resourceCreateParam.LocationRoot = GameDefine.AssetRootPath;
		resourceCreateParam.SimulationOnEditor = SimulationOnEditor;
		resourceCreateParam.BundleServices = bundleServices;
		resourceCreateParam.DecryptServices = new Decrypter();
		resourceCreateParam.AutoReleaseInterval = 1f;
		MotionEngine.CreateModule<ResourceManager>(resourceCreateParam);

		if (SkipCDN)
		{
			// 开始游戏
			StartGame();
		}
		else
		{
			// 初始化补丁窗口
			PatchWindow.Instance.Initialize();
			WaitForSeconds waiting = new WaitForSeconds(0.5f);
			yield return waiting;

			// 开始补丁更新流程
			PatchManager.Instance.Download();
		}
	}

	/// <summary>
	/// 事件处理
	/// </summary>
	private void OnHandleEventMessage(IEventMessage msg)
	{
		if (msg is PatchEventMessageDefine.PatchStatesChange)
		{
			var message = msg as PatchEventMessageDefine.PatchStatesChange;
			if (message.CurrentStates == EPatchStates.PatchDone)
			{
				// 销毁补丁窗口
				PatchWindow.Instance.Destroy();

				// 注意：在补丁下载结束之后，一定要强制释放资源管理器里所有的资源。
				ResourceManager.Instance.UnloadAllAssets();

				// 开始游戏
				StartGame();
			}
		}
		else if (msg is PatchEventMessageDefine.OperationEvent)
		{
			PatchManager.Instance.HandleEventMessage(msg);
		}
	}

	// 开始游戏
	private void StartGame()
	{
		MotionEngine.CreateModule<Demo>();
		Demo.Instance.StartGame();
	}
}