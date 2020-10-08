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
	private AssetOperationHandle _handle1;
	private AssetOperationHandle _handle2;

	void IModule.OnCreate(object createParam)
	{
	}
	void IModule.OnUpdate()
	{
		if(Input.GetKeyDown(KeyCode.G))
		{
			ResourceManager.Instance.Release(_handle1);
			ResourceManager.Instance.Release(_handle2);
		}
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
		var rootHandle = ResourceManager.Instance.LoadAssetAsync<GameObject>("UIPanel/UIRoot");
		yield return rootHandle;	
		GameObject uiRoot = rootHandle.InstantiateObject; // 实例化对象

		// 加载窗口
		GameLog.Log("Load LoginWindow");
		GameObject window;
		{
			var handle = ResourceManager.Instance.LoadAssetAsync<GameObject>("UIPanel/LoginWindow");
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
			_handle1 = ResourceManager.Instance.LoadAssetAsync<Texture>("UITexture/Foods/eggs");
			yield return _handle1;
			Texture tex1 = _handle1.AssetObject as Texture;

			_handle2 = ResourceManager.Instance.LoadAssetAsync<Texture>("UITexture/Foods/apple");
			yield return _handle2;
			Texture tex2 = _handle2.AssetObject as Texture;

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
			GameLog.Log("Load Monster");
			var handle = ResourceManager.Instance.LoadAssetAsync<GameObject>("Entity/Monster/Boss");
			yield return handle;
			var sphere = handle.InstantiateObject; // 实例化对象
			sphere.transform.position = new Vector3(5f, 0, 0);
			sphere.transform.localScale = sphere.transform.localScale * 2f;
		}
	}
}