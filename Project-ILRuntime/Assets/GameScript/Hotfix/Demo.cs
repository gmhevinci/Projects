using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Resource;
using MotionFramework.Network;

namespace Hotfix
{
	public class Demo
	{
		public static readonly Demo Instance = new Demo();
		private bool _isSendLogin = false;

		public void Start()
		{
			var btn = GameObject.Find("Button").GetComponent<Button>();
			btn.onClick.AddListener(OnClickButton);

			// 加载资源
			AssetReference assetRef = new AssetReference("Entity/Sphere");
			var handle = assetRef.LoadAssetAsync<GameObject>();
			handle.Completed += Handle_Completed;
		}
		public void Update()
		{
			if(NetworkManager.Instance.States == ENetworkStates.Connected)
			{
				if(_isSendLogin  == false)
				{
					_isSendLogin = true;
					C2R_Login msg = new C2R_Login
					{
						RpcId = 100,
						Account = "hello",
						Password = "1234"
					};
					HotfixNetManager.Instance.SendHotfixMsg(msg);
				}
			}
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
	}
}