using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Config;
using MotionFramework.Window;
using MotionFramework.Event;

namespace Hotfix
{
	public class Demo
	{
		public static readonly Demo Instance = new Demo();

		public void Start()
		{
			HotfixNetManager.Instance.Create();
			FsmManager.Instance.Create();

			// 缓存所有的特性
			HotfixLog.Log("收集所有热更类的属性并缓存");
			{
				Attribute attribute1 = HotfixTypeHelper.GetAttribute<WindowAttribute>(typeof(UILogin));
				ILRManager.Instance.CacheHotfixAttribute(typeof(UILogin), attribute1);
				Attribute attribute2 = HotfixTypeHelper.GetAttribute<WindowAttribute>(typeof(UIMain));
				ILRManager.Instance.CacheHotfixAttribute(typeof(UIMain), attribute2);
				Attribute attribute3 = HotfixTypeHelper.GetAttribute<WindowAttribute>(typeof(UIMap));
				ILRManager.Instance.CacheHotfixAttribute(typeof(UIMap), attribute3);
			}

			// 开启协程加载资源
			MotionEngine.StartCoroutine(AsyncLoadAssets());

			// 测试热更事件
			HotfixEventManager.Instance.AddListener<HotfixEventDefine.ClickSkill1Event>(OnHandleEventMessage);
			HotfixEventManager.Instance.AddListener<HotfixEventDefine.ClickSkill2Event>(OnHandleEventMessage);
			HotfixEventManager.Instance.AddListener<HotfixEventDefine.ClickSkill3Event>(OnHandleEventMessage);
			HotfixEventManager.Instance.AddListener<HotfixEventDefine.ClickSkill4Event>(OnHandleEventMessage);
			HotfixEventManager.Instance.AddListener<HotfixEventDefine.ClickSkill5Event>(OnHandleEventMessage);
		}
		public void Update()
		{
			HotfixNetManager.Instance.Update();
			FsmManager.Instance.Update();
		}

		public IEnumerator AsyncLoadAssets()
		{
			// 加载UIRoot
			HotfixLog.Log("开始加载UIRoot");
			var uiRoot = WindowManager.Instance.CreateUIRoot<CanvasRoot>("UIPanel/UIRoot");
			yield return uiRoot;

			// 加载窗口
			HotfixLog.Log("开始加载登录窗口");
			yield return WindowManager.Instance.OpenWindow(typeof(UILogin), "UIPanel/UILogin");

			// 加载模型
			HotfixLog.Log("开始加载模型");
			var handle = ResourceManager.Instance.LoadAssetAsync<GameObject>("Entity/Sphere");
			yield return handle;

			var sphere = handle.InstantiateObject;
			sphere.transform.position = Vector3.zero;
			sphere.transform.localScale = Vector3.one * 3f;
			HotfixLog.Log($"模型名字：{sphere.name}");

			// 加载配表
			HotfixLog.Log("开始加载配表");
			yield return ConfigManager.Instance.LoadConfig(typeof(CfgAvatar), "Config/Avatar");

			CfgAvatar config = ConfigManager.Instance.GetConfig(typeof(CfgAvatar)) as CfgAvatar;
			CfgAvatarTable table = config.GetTable(1001) as CfgAvatarTable;
			HotfixLog.Log($"表格数据：{table.HeadIcon}");
			HotfixLog.Log($"表格数据：{table.Model}");
		}

		private void OnHandleEventMessage(IEventMessage msg)
		{
			Debug.Log($"Click Skill = {msg.GetType()}");
		}
	}
}