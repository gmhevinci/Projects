using System;
using System.Collections;
using System.Collections.Generic;
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

	/// <summary>
	/// 在编辑器下模拟运行
	/// </summary>
	public bool SimulationOnEditor = false;

	/// <summary>
	/// 是否跳过CDN服务器
	/// </summary>
	public bool SkipCDN = false;

	public EQuality Quality = EQuality.Default;
	public ELanguage Language = ELanguage.Default;


	void Awake()
	{
#if !UNITY_EDITOR
		SimulationOnEditor = false;
#endif

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
		IBundleServices bundleServices = null;
		if (SkipCDN)
		{
			var localBundleServices = new LocalBundleServices(variantRules);
			yield return localBundleServices.InitializeAsync(SimulationOnEditor);
			bundleServices = localBundleServices;
		}
		else
		{
			// 远程服务器信息
			string webServer = "http://127.0.0.1";
			string cdnServer = "http://127.0.0.1";
			string defaultWebServerIP = $"{webServer}/WEB/PC/GameVersion.php";
			string defaultCDNServerIP = $"{cdnServer}/CDN/PC";
			RemoteServerInfo serverInfo = new RemoteServerInfo(defaultWebServerIP, defaultCDNServerIP);
			serverInfo.AddServerInfo(RuntimePlatform.Android, $"{webServer}/WEB/Android/GameVersion.php", $"{cdnServer}/CDN/Android");
			serverInfo.AddServerInfo(RuntimePlatform.IPhonePlayer, $"{webServer}/WEB/Iphone/GameVersion.php", $"{cdnServer}/CDN/Iphone");

			var patchCreateParam = new PatchManager.CreateParameters();
			patchCreateParam.ServerID = PlayerPrefs.GetInt("SERVER_ID_KEY", 0);
			patchCreateParam.ChannelID = 0;
			patchCreateParam.DeviceID = 0;
			patchCreateParam.TestFlag = PlayerPrefs.GetInt("TEST_FLAG_KEY", 0);
			patchCreateParam.CheckLevel = ECheckLevel.CheckSize;
			patchCreateParam.ServerInfo = serverInfo;
			patchCreateParam.VariantRules = variantRules;

			PatchManager patchManager = MotionEngine.CreateModule<PatchManager>(patchCreateParam);
			yield return patchManager.InitializeAync();
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
		resourceCreateParam.AutoReleaseInterval = 10f;
		MotionEngine.CreateModule<ResourceManager>(resourceCreateParam);

		if (SkipCDN)
		{
			// 开始游戏
			StartGame();
		}
		else
		{
			// 初始化补丁窗口
			yield return PatchWindow.Instance.InitializeAsync();
			WaitForSeconds waiting = new WaitForSeconds(1f);
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

			// 补丁下载完毕
			// 注意：在补丁下载结束之后，一定要强制释放资源管理器里所有的资源，还有重新载入Unity清单。
			if (message.CurrentStates == EPatchStates.DownloadOver)
			{
				PatchWindow.Instance.Destroy();
				ResourceManager.Instance.ForceReleaseAll();

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