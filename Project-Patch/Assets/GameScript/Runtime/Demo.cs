using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Patch;
using MotionFramework.Scene;

public class Demo : ModuleSingleton<Demo>, IModule
{
	/// <summary>
	/// 当前进行的关卡
	/// </summary>
	public int PlayLevel { set; get; }

	void IModule.OnCreate(object createParam)
	{
	}
	void IModule.OnUpdate()
	{
	}
	void IModule.OnGUI()
	{
	}

	public void StartGame()
	{
		GameLog.Log("Hello game world.");
		SceneManager.Instance.ChangeMainScene("Scene/Login", true, null);
	}
}