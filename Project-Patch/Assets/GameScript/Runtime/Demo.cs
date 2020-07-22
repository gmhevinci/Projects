using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Patch;

public class Demo : ModuleSingleton<Demo>, IModule
{
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
		MotionEngine.StartCoroutine(LoadAssets());
	}

	private IEnumerator LoadAssets()
	{
		// 加载UI面板
		GameLog.Log("Load UIRoot.");
		AssetReference rootRef = new AssetReference("UIPanel/UIRoot");
		var rootHandle = rootRef.LoadAssetAsync<GameObject>();
		yield return rootHandle;	
		GameObject uiRoot = rootHandle.InstantiateObject; // 实例化对象

		// 加载窗口
		GameObject window;
		{
			AssetReference windowRef = new AssetReference("UIPanel/LoginWindow");
			var handle = windowRef.LoadAssetAsync<GameObject>();
			yield return handle;
			window = handle.InstantiateObject; // 实例化对象
			window.transform.SetParent(uiRoot.transform, false);

			var versionTxt = window.transform.BFSearch("Version").GetComponent<Text>();
			if (MotionEngine.Contains(typeof(PatchManager)))
				versionTxt.text = PatchManager.Instance.GetRequestedGameVersion().ToString();
			else
				versionTxt.text = "NO Server";
		}

		// 加载资源包
		{
			GameLog.Log("Load texture package");
			AssetReference packRef = new AssetReference("UITexture/Foods");
			var handle1 = packRef.LoadAssetAsync<Texture>("eggs");
			yield return handle1;
			Texture tex1 = handle1.AssetObject as Texture;

			var handle2 = packRef.LoadAssetAsync<Texture>("apple");
			yield return handle2;
			Texture tex2 = handle2.AssetObject as Texture;

			// 设置纹理1
			RawImage img1 = window.transform.BFSearch("FoodImg1").GetComponent<RawImage>();	
			img1.texture = tex1;
			img1.SetNativeSize();

			// 设置纹理2
			RawImage img2 = window.transform.BFSearch("FoodImg2").GetComponent<RawImage>();
			img2.texture = tex2;
			img2.SetNativeSize();
		}

		// 加载模型
		{
			AssetReference entityRef = new AssetReference("Entity/Monster/Boss");
			var handle = entityRef.LoadAssetAsync<GameObject>();
			yield return handle;
			var sphere = handle.InstantiateObject; // 实例化对象
			sphere.transform.position = new Vector3(5f, 0, 0);
			sphere.transform.localScale = sphere.transform.localScale * 2f;
		}
	}
}