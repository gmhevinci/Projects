using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Window;
using ILRuntime.Runtime.Intepreter;

public static class UITools
{
	public static void PreLoadWindow(string typeName, string location)
	{
		UIWindow instance = (UIWindow)ILRManager.Instance.ILRDomain.Instantiate(typeName).CLRInstance;
		WindowManager.Instance.PreloadWindow(instance, location);
	}
}