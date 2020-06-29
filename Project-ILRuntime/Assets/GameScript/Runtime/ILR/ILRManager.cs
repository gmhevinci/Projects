using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Console;
using MotionFramework.Patch;
using MotionFramework.Window;

public class ILRManager : ModuleSingleton<ILRManager>, IModule, IActivatorServices
{
	/// <summary>
	/// 游戏模块创建参数
	/// </summary>
	public class CreateParameters
	{
		/// <summary>
		/// 是否启用ILRuntime，否则使用mono模式运行
		/// </summary>
		public bool IsEnableILRuntime;

		/// <summary>
		/// 编辑器的模拟运行
		/// </summary>
		public bool SimulationOnEditor;
	}

	private MemoryStream _dllStream;
	private MemoryStream _pdbStream;
	private Assembly _monoAssembly;
	private bool _isEnableILRuntime;
	private bool _isSimulationOnEditor;

	// 热更新层相关函数
	private IStaticMethod _startFun;
	private IStaticMethod _updateFun;
	private IStaticMethod _lateUpdateFun;
	private IStaticMethod _uiLanguageFun;

	/// <summary>
	/// 热更新的程序域
	/// </summary>
	public ILRuntime.Runtime.Enviorment.AppDomain ILRDomain { private set; get; }

	/// <summary>
	/// 热更新所有类型集合
	/// </summary>
	public List<Type> HotfixAssemblyTypes { private set; get; }


	void IModule.OnCreate(System.Object param)
	{
		CreateParameters createParam = param as CreateParameters;
		if (createParam == null)
			throw new Exception($"{nameof(ILRManager)} create param is invalid.");

		_isEnableILRuntime = createParam.IsEnableILRuntime;
		_isSimulationOnEditor = createParam.SimulationOnEditor;
	}
	void IModule.OnUpdate()
	{
		if (_updateFun != null)
			_updateFun.Invoke();
	}
	void IModule.OnGUI()
	{
		ConsoleGUI.Lable($"[{nameof(ILRManager)}] EnableILRuntime : {_isEnableILRuntime}");
	}

	/// <summary>
	/// 开始游戏
	/// </summary>
	public void StartGame()
	{
		if (Application.isEditor || Debug.isDebugBuild)
			LoadHotfixAssemblyWithPDB();
		else
			LoadHotfixAssembly();

		InitHotfixProgram();

		if (_startFun != null)
			_startFun.Invoke();
	}

	/// <summary>
	/// 释放资源
	/// </summary>
	public void ReleaseILRuntime()
	{
		if (_dllStream != null)
		{
			_dllStream.Close();
			_dllStream = null;
		}
		if (_pdbStream != null)
		{
			_pdbStream.Close();
			_pdbStream = null;
		}
	}

	/// <summary>
	/// 从热更配备里获取界面多语言
	/// </summary>
	public string UILanguage(string key)
	{
		return (string)_uiLanguageFun.Invoke(key);
	}

	// 加载热更的动态库文件
	private void LoadHotfixAssembly()
	{
		TextAsset dllAsset = LoadDLL();

		if (_isEnableILRuntime)
		{
			_dllStream = new MemoryStream(dllAsset.bytes);
			ILRDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
			ILRDomain.LoadAssembly(_dllStream, null, null);
		}
		else
		{
#if ENABLE_IL2CPP
			throw new NotImplementedException("You must enable ILRuntime when with IL2CPP mode.");
#endif
			_monoAssembly = Assembly.Load(dllAsset.bytes, null);
		}
	}
	private void LoadHotfixAssemblyWithPDB()
	{
		TextAsset dllAsset = LoadDLL();
		TextAsset pdbAsset = LoadPDB();

		if (_isEnableILRuntime)
		{
			_dllStream = new MemoryStream(dllAsset.bytes);
			_pdbStream = new MemoryStream(pdbAsset.bytes);
			var symbolReader = new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider();
			ILRDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
			ILRDomain.LoadAssembly(_dllStream, _pdbStream, symbolReader);
		}
		else
		{
#if ENABLE_IL2CPP
			throw new NotImplementedException("You must enable ILRuntime when with IL2CPP mode.");
#endif
			_monoAssembly = Assembly.Load(dllAsset.bytes, pdbAsset.bytes);
		}
	}
	private TextAsset LoadDLL()
	{
		string location = $"Assembly/{ILRDefine.StrMyHotfixDLLFileName}";
		return LoadAsset(location);
	}
	private TextAsset LoadPDB()
	{
		string location = $"Assembly/{ILRDefine.StrMyHotfixPDBFileName}";
		return LoadAsset(location);
	}

	// 初始化热更程序
	private void InitHotfixProgram()
	{
		string typeName = "Hotfix.Main";
		string startFunName = "Start";
		string updateFunName = "Update";
		string lateUpdateFunName = "LateUpdate";
		string uiLanguageFunName = "UILanguage";

		if (_isEnableILRuntime)
		{
			ILRRegister.Register(ILRDomain);
			_startFun = new ILRStaticMethod(ILRDomain, typeName, startFunName, 0);
			_updateFun = new ILRStaticMethod(ILRDomain, typeName, updateFunName, 0);
			_lateUpdateFun = new ILRStaticMethod(ILRDomain, typeName, lateUpdateFunName, 0);
			_uiLanguageFun = new ILRStaticMethod(ILRDomain, typeName, uiLanguageFunName, 1);
			HotfixAssemblyTypes = ILRDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
		}
		else
		{
			Type type = _monoAssembly.GetType(typeName);
			_startFun = new MonoStaticMethod(type, startFunName);
			_updateFun = new MonoStaticMethod(type, updateFunName);
			_lateUpdateFun = new MonoStaticMethod(type, lateUpdateFunName);
			_uiLanguageFun = new MonoStaticMethod(type, uiLanguageFunName);
			HotfixAssemblyTypes = _monoAssembly.GetTypes().ToList<Type>();
		}
	}

	/// <summary>
	/// 同步加载程序集文件
	/// </summary>
	private TextAsset LoadAsset(string location)
	{
		string loadPath = ResourceManager.Instance.GetLoadPath(location);
		if (_isSimulationOnEditor)
		{
#if UNITY_EDITOR
			return UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(loadPath);
#else
			throw new Exception($"AssetSystem simulation only support unity editor.");
#endif
		}
		else
		{
			AssetBundle bundle = AssetBundle.LoadFromFile(loadPath);
			if (bundle == null)
				return null;

			string fileName = Path.GetFileName(location);
			var result = bundle.LoadAsset<TextAsset>(fileName);
			bundle.Unload(false); // 注意：这里要卸载AssetBundle
			return result;
		}
	}

	#region 反射服务接口
	private readonly Dictionary<Type, Attribute> _cacheAttributes = new Dictionary<Type, Attribute>();

	/// <summary>
	/// 缓存热更新特性
	/// </summary>
	public void CacheHotfixAttribute(Type type, Attribute attribute)
	{
		_cacheAttributes.Add(type, attribute);
	}

	object IActivatorServices.CreateInstance(Type type)
	{
		if (_isEnableILRuntime)
			return ILRDomain.Instantiate(type.FullName).CLRInstance;
		else
			return Activator.CreateInstance(type);
	}
	Attribute IActivatorServices.GetAttribute(Type type)
	{
		_cacheAttributes.TryGetValue(type, out Attribute result);
		return result;
	}
	#endregion
}