using System;
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
	public static GameLauncher Instance = null;
	private IMotionEngine _motionEngine;

	[Tooltip("是否跳过CDN服务器")]
	public bool SkipCDN = true;

	[Tooltip("资源系统的加载模式")]
	public EAssetSystemMode AssetSystemMode = EAssetSystemMode.ResourcesMode;

	void Awake()
	{
		Instance = this;

		// 不销毁游戏对象
		DontDestroyOnLoad(gameObject);

		// 注册日志系统
		AppLog.RegisterCallback(HandleMotionFrameworkLog);

		// 初始化框架
		_motionEngine = AppEngine.Instance;
		_motionEngine.Initialize(this);

		// 初始化控制台
		if (Application.isEditor || Debug.isDebugBuild)
			AppConsole.Initialize();

		// 初始化应用
		InitAppliaction();
	}
	void Start()
	{
		CreateGameModules();
	}
	void Update()
	{
		_motionEngine.OnUpdate();
	}
	void OnGUI()
	{
		if (Application.isEditor || Debug.isDebugBuild)
			AppConsole.DrawGUI();
	}

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
	private void HandleMotionFrameworkLog(ELogType logType, string log)
	{
		if (logType == ELogType.Log)
		{
			UnityEngine.Debug.Log(log);
		}
		else if (logType == ELogType.Error)
		{
			UnityEngine.Debug.LogError(log);
		}
		else if (logType == ELogType.Warning)
		{
			UnityEngine.Debug.LogWarning(log);
		}
		else if (logType == ELogType.Exception)
		{
			UnityEngine.Debug.LogError(log);
		}
		else
		{
			throw new NotImplementedException($"{logType}");
		}
	}

	private void CreateGameModules()
	{
		// 创建事件管理器
		AppEngine.Instance.CreateModule<EventManager>();

		// 创建网络管理器
		AppEngine.Instance.CreateModule<NetworkManager>();

		// 创建资源管理器
		var resourceCreateParam = new ResourceManager.CreateParameters();
		resourceCreateParam.AssetRootPath = GameDefine.AssetRootPath;
		resourceCreateParam.AssetSystemMode = AssetSystemMode;
		resourceCreateParam.BundleServices = null;
		AppEngine.Instance.CreateModule<ResourceManager>(resourceCreateParam);

		// 创建音频管理器
		AppEngine.Instance.CreateModule<AudioManager>();

		// 创建场景管理器
		AppEngine.Instance.CreateModule<SceneManager>();

		// 创建对象池管理器
		AppEngine.Instance.CreateModule<PoolManager>();

		// 直接进入游戏
		AppEngine.Instance.CreateModule<LuaManager>();	
	}
}