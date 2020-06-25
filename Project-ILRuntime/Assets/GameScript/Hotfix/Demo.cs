using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Resource;
using MotionFramework.Network;
using MotionFramework.Event;

namespace Hotfix
{
	public class Demo
	{
		public static readonly Demo Instance = new Demo();

		public void Start()
		{
			// UGUI相关
			var btn = GameObject.Find("Button").GetComponent<Button>();
			btn.onClick.AddListener(OnClickButton);

			// 监听事件
			EventManager.Instance.AddListener<NetworkEventMessageDefine.ConnectFail>(OnHandleEventMessage);
			EventManager.Instance.AddListener<NetworkEventMessageDefine.ConnectSuccess>(OnHandleEventMessage);

			// 创建状态机
			FsmManager.Instance.Create();

			// 加载资源
			AssetReference assetRef = new AssetReference("Entity/Sphere");
			var handle = assetRef.LoadAssetAsync<GameObject>();
			handle.Completed += Handle_Completed;
		}
		public void Update()
		{
			// 更新状态机
			FsmManager.Instance.Update();
		}

		private void OnClickButton()
		{
			// 尝试连接ET服务器
			NetworkManager.Instance.ConnectServer("127.0.0.1", 10002);
		}

		private void Handle_Completed(AssetOperationHandle obj)
		{
			var sphere = obj.InstantiateObject;
			sphere.transform.position = Vector3.zero;
			sphere.transform.localScale = Vector3.one * 3f;
		}

		private void OnHandleEventMessage(IEventMessage msg)
		{
			if(msg is NetworkEventMessageDefine.ConnectFail)
			{
				HotfixLogger.Log("连接服务器失败");
			}
			else if(msg is NetworkEventMessageDefine.ConnectSuccess)
			{
				HotfixLogger.Log("连接服务器成功");

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
	}
}