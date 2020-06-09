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
		MotionEngine.StartCoroutine(LoadAsset());	
	}

	private IEnumerator LoadAsset()
	{
		// 加载UI面板
		GameLogger.Log("Load UICanvas.");
		AssetReference canvasRef = new AssetReference("UIPanel/UICanvas");
		var canvasHandle = canvasRef.LoadAssetAsync<GameObject>();
		yield return canvasHandle;

		// 获取组件
		GameObject uiRoot = canvasHandle.InstantiateObject;
		Image img = uiRoot.transform.BFSearch("Image").GetComponent<Image>();

		// 加载图集
		GameLogger.Log("Load UIAtlas.");
		AssetReference atlasRef = new AssetReference("UIAtlas/UIWordArt/UIWordArt", "CN");
		var atlasHandle = atlasRef.LoadAssetAsync<SpriteAtlas>();
		yield return atlasHandle;

		// 设置精灵
		SpriteAtlas atlas = atlasHandle.AssetObject as SpriteAtlas;
		img.sprite = atlas.GetSprite("login_title");
		img.SetNativeSize();

		// 加载实体
		GameLogger.Log("Load Cube.");
		AssetReference entityRef = new AssetReference("Entity/Cube");
		var entityHandle = entityRef.LoadAssetAsync<GameObject>();
		yield return entityHandle;

		// 实例化实体
		GameObject cube = entityHandle.InstantiateObject;
		cube.transform.position = Vector3.zero;
		cube.transform.localScale = Vector3.one * 5f;
	}
}