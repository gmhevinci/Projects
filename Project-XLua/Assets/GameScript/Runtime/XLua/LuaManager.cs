﻿using System;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;
using MotionFramework;
using MotionFramework.Console;
using MotionFramework.Resource;
using MotionFramework.Network;
using MotionFramework.Utility;
using MotionFramework.Patch;

public class LuaManager : ModuleSingleton<LuaManager>, IModule
{
	[CSharpCallLua]
	public delegate string LanguageDelegate(string key);
	[CSharpCallLua]
	public delegate void NetMessageDelegate(int msgID, byte[] bytes);
	

	private readonly LuaEnv _luaEnv = new LuaEnv();
	private readonly Timer _tickTimer = Timer.CreatePepeatTimer(0, 1f);

	private LuaTable _gameTable;
	private Action _funStart;
	private Action _funUpdate;
	private LanguageDelegate _funLanguage;
	private NetMessageDelegate _funNetMessage;

	void IModule.OnCreate(object createParam)
	{
		_luaEnv.AddLoader(CustomLoaderMethod);
		_luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
		_luaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
		_luaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);

		// 监听热更网络数据
		NetworkManager.Instance.HotfixPackageCallback += OnHandleHotfixPackage;
	}
	void IModule.OnUpdate()
	{
		// Update
		_funUpdate?.Invoke();

		// Tick
		if (_tickTimer.Update(Time.unscaledDeltaTime))
			_luaEnv.Tick();
	}
	void IModule.OnGUI()
	{
		ConsoleGUI.Lable($"[{nameof(LuaManager)}] Lua memory : {_luaEnv.Memroy}Kb");
	}

	public void StartGame()
	{
		// 初始化
		InitLuaScript();

		// Start
		_funStart?.Invoke();
	}

	/// <summary>
	/// 多语言查询
	/// </summary>
	/// <param name="key">关键字</param>
	/// <returns>返回查询结果</returns>
	public string Language(string key)
	{
		return _funLanguage?.Invoke(key);
	}

	/// <summary>
	/// 发送热更新网络消息
	/// </summary>
	public void SendHotfixNetMessage(int msgID, byte[] bytes)
	{
		DefaultNetworkPackage package = new DefaultNetworkPackage();
		package.IsHotfixPackage = true;
		package.MsgID = msgID;
		package.BodyBytes = bytes;
		NetworkManager.Instance.SendMessage(package);
	}

	/// <summary>
	/// 初始化LUA脚本
	/// </summary>
	private void InitLuaScript()
	{
		TextAsset asset = LoadAsset("Lua/Main.lua");
		_gameTable = ExecuteScript(asset.bytes, "Main") as LuaTable;
		_funStart = _gameTable.Get<Action>("Start");
		_funUpdate = _gameTable.Get<Action>("Update");
		_funLanguage = _gameTable.Get<LanguageDelegate>("Language");
		_funNetMessage = _gameTable.Get<NetMessageDelegate>("HandleNetMessage");
	}

	/// <summary>
	/// 自定义文件加载方法
	/// </summary>
	private byte[] CustomLoaderMethod(ref string fileName)
	{
		string location = $"Lua/{fileName}.lua";
		TextAsset asset = LoadAsset(location);
		if(asset == null)
		{
			Debug.LogWarning($"Failed to load lua file : {location}");
			return null;
		}
		return asset.bytes;
	}

	/// <summary>
	/// Execute lua script directly
	/// </summary>
	private object ExecuteScript(byte[] scriptCode, string chunkName = "code")
	{
		var results = _luaEnv.DoString(Encoding.UTF8.GetString(scriptCode), chunkName);
		if (results == null) return null;
		if (results.Length == 1)
		{
			return results[0];
		}
		else
		{
			return results;
		}
	}

	private void OnHandleHotfixPackage(INetworkPackage pack)
	{
		DefaultNetworkPackage package = pack as DefaultNetworkPackage;
		_funNetMessage(package.MsgID, package.BodyBytes);
	}

	/// <summary>
	/// 同步加载LUA文件
	/// </summary>
	private TextAsset LoadAsset(string location)
	{
		var handle =  ResourceManager.Instance.LoadAssetSync<TextAsset>(location);
		return handle.AssetObject as TextAsset;
	}
}