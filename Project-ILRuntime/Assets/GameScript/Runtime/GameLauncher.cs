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
using MotionFramework.Window;

public class GameLauncher : MonoBehaviour
{
	[Tooltip("启用脚本热更模式")]
	public bool EnableILRuntime = true;

	[Tooltip("在编辑器下模拟运行")]
	public bool SimulationOnEditor = true;

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

		// 创建网络管理器
		var networkCreateParam = new NetworkManager.CreateParameters();
		networkCreateParam.PackageCoderType = typeof(ProtoPackageCoder);
		networkCreateParam.PackageMaxSize = ushort.MaxValue;
		MotionEngine.CreateModule<NetworkManager>(networkCreateParam);

		// 本地资源服务接口
		LocalBundleServices bundleServices = new LocalBundleServices();
		yield return bundleServices.InitializeAsync(SimulationOnEditor);

		// 创建资源管理器
		var resourceCreateParam = new ResourceManager.CreateParameters();
		resourceCreateParam.LocationRoot = GameDefine.AssetRootPath;
		resourceCreateParam.SimulationOnEditor = SimulationOnEditor;
		resourceCreateParam.BundleServices = bundleServices;
		resourceCreateParam.DecryptServices = null;
		resourceCreateParam.AutoReleaseInterval = 10f;
		MotionEngine.CreateModule<ResourceManager>(resourceCreateParam);

		// 创建配表管理器
		MotionEngine.CreateModule<ConfigManager>();

		// 创建音频管理器
		MotionEngine.CreateModule<AudioManager>();

		// 创建场景管理器
		MotionEngine.CreateModule<SceneManager>();

		// 创建窗口管理器
		MotionEngine.CreateModule<WindowManager>();

		// 创建ILR管理器
		ILRManager.CreateParameters createParameters = new ILRManager.CreateParameters();
		createParameters.IsEnableILRuntime = EnableILRuntime;
		createParameters.SimulationOnEditor = SimulationOnEditor;
		MotionEngine.CreateModule<ILRManager>(createParameters);

		// 反射服务服务接口
		ConfigManager.Instance.ActivatorServices = ILRManager.Instance;
		WindowManager.Instance.ActivatorServices = ILRManager.Instance;

		// 开始游戏
		ILRManager.Instance.StartGame();
	}
}