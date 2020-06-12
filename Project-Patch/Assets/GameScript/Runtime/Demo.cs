using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using MotionFramework;
using MotionFramework.Resource;

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
		GameLogger.Log("Hello game world.");
		MotionEngine.StartCoroutine(LoadAssets());
	}

	private IEnumerator LoadAssets()
	{
		// 加载UI面板
		GameLogger.Log("Load UIRoot.");
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
		}

		// 加载图集
		{
			GameLogger.Log("Load UIAtlas.");
			AssetReference atlasRef = new AssetReference("UIAtlas/UIWordArt", "KR");
			var handle = atlasRef.LoadAssetAsync<SpriteAtlas>();
			yield return handle;
			SpriteAtlas spriteAtlas = handle.AssetObject as SpriteAtlas;

			// 设置精灵
			Image img = window.transform.BFSearch("WordArt").GetComponent<Image>();
			img.sprite = spriteAtlas.GetSprite("login_title");
			img.SetNativeSize();
		}

		// 加载资源包
		{
			GameLogger.Log("Load texture package");
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
	}
}