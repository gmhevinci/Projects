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
		GameLogger.Log("Load UICanvas.");
		AssetReference canvasRef = new AssetReference("UIPanel/UICanvas");
		var canvasHandle = canvasRef.LoadAssetAsync<GameObject>();
		yield return canvasHandle;

		// 实例化UI面板
		GameObject uiRoot = canvasHandle.InstantiateObject;

		// 加载图集
		{
			GameLogger.Log("Load UIAtlas.");
			AssetReference atlasRef = new AssetReference("UIAtlas/UIWordArt/UIWordArt", "CN");
			var atlasHandle = atlasRef.LoadAssetAsync<SpriteAtlas>();
			yield return atlasHandle;

			// 从图集里设置按钮精灵	
			Image img = uiRoot.transform.BFSearch("Button").GetComponent<Image>();
			SpriteAtlas spriteAtlas = atlasHandle.AssetObject as SpriteAtlas;
			img.sprite = spriteAtlas.GetSprite("login_title");
			img.SetNativeSize();
		}

		// 加载资源包
		{
			GameLogger.Log("Load texture package");
			AssetReference packRef = new AssetReference("UITexture/Foods");
			var handle1 = packRef.LoadAssetAsync<Texture>("eggs");
			yield return handle1;
			var handle2 = packRef.LoadAssetAsync<Texture>("banana");
			yield return handle2;

			// 设置纹理图片1
			RawImage img1 = uiRoot.transform.BFSearch("Image1").GetComponent<RawImage>();
			Texture tex1 = handle1.AssetObject as Texture;
			img1.texture = tex1;
			img1.SetNativeSize();

			// 设置纹理图片2
			RawImage img2 = uiRoot.transform.BFSearch("Image2").GetComponent<RawImage>();
			Texture tex2 = handle2.AssetObject as Texture;
			img2.texture = tex2;
			img2.SetNativeSize();
		}
	}
}