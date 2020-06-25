using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Config;
using MotionFramework.Window;

namespace Hotfix
{
	public class Demo
	{
		public static readonly Demo Instance = new Demo();

		public void Start()
		{
			HotfixNetManager.Instance.Create();
			FsmManager.Instance.Create();

			// 开启协程加载资源
			MotionEngine.StartCoroutine(LoadAssets());
		}

		public IEnumerator LoadAssets()
		{
			// 加载UIRoot
			var uiRoot = WindowManager.Instance.CreateUIRoot<CanvasRoot>("UIPanel/UIRoot");
			yield return uiRoot;

			// 加载窗口
			HotfixLogger.Log("开始加载窗口");
			UITools.PreLoadWindow("Hotfix.UILogin", "UIPanel/LoginWindow");
			//UILogin login = new UILogin();
			//WindowManager.Instance.PreloadWindow(login, "UIPanel/LoginWindow");

			// 加载模型
			HotfixLogger.Log("开始加载模型");
			AssetReference assetRef = new AssetReference("Entity/Sphere");
			var handle = assetRef.LoadAssetAsync<GameObject>();
			yield return handle;

			var sphere = handle.InstantiateObject;
			sphere.transform.position = Vector3.zero;
			sphere.transform.localScale = Vector3.one * 3f;
			HotfixLogger.Log($"模型名字：{sphere.name}");

			// 加载配表
			HotfixLogger.Log("开始加载配表");
			CfgAvatar cfgInstance = new CfgAvatar();
			yield return ConfigManager.Instance.LoadConfig(cfgInstance, "Config/Avatar");

			CfgAvatarTable table = cfgInstance.GetTable(1001) as CfgAvatarTable;
			HotfixLogger.Log($"表格数据：{table.HeadIcon}");
			HotfixLogger.Log($"表格数据：{table.Model}");
		}

		public void Update()
		{
			HotfixNetManager.Instance.Update();
			FsmManager.Instance.Update();
		}
	}
}