using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Event;
using MotionFramework.Window;
using MotionFramework.Network;

namespace Hotfix
{
	[Window((int)EWindowLayer.Panel, true)]
	sealed class UILogin : CanvasWindow
	{
		public override void OnCreate()
		{
			// 监听按钮点击事件
			AddButtonListener("LoginWindow/Button", OnClickLogin);

			// 监听事件
			EventManager.Instance.AddListener<NetworkEventMessageDefine.ConnectFail>(OnHandleEventMessage);
			EventManager.Instance.AddListener<NetworkEventMessageDefine.ConnectSuccess>(OnHandleEventMessage);
		}
		public override void OnDestroy()
		{
		}
		public override void OnRefresh()
		{
		}
		public override void OnUpdate()
		{
		}

		private void OnHandleEventMessage(IEventMessage msg)
		{
			if (msg is NetworkEventMessageDefine.ConnectFail)
			{
				HotfixLog.Log("连接服务器失败");
			}
			else if (msg is NetworkEventMessageDefine.ConnectSuccess)
			{
				HotfixLog.Log("连接服务器成功");

				// 发送登录消息
				C2R_Login loginMsg = new C2R_Login
				{
					RpcId = 100,
					Account = "hello",
					Password = "1234"
				};
				HotfixNetManager.Instance.SendHotfixMsg(loginMsg);
			}
		}

		private void OnClickLogin()
		{
			if (NetworkManager.Instance.States == ENetworkStates.Connecting)
				return;

			// 尝试连接ET服务器
			//HotfixLog.Log("开始连接服务器");
			//NetworkManager.Instance.ConnectServer("127.0.0.1", 10002);

			// 打开新的窗口
			UITools.OpenWindow<UIMain>();
			UITools.CloseWindow<UILogin>();
		}
	}
}