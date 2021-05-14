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
		GameObject uiRoot = GameObject.Find("UIRoot");

		// 加载窗口
		GameLog.Log("Load Window");
		GameObject window;
		{
			var handle = ResourceManager.Instance.LoadAssetAsync<GameObject>("UIPanel/LoginWindow");
			yield return handle;
			window = handle.InstantiateObject; // 实例化对象
			window.transform.SetParent(uiRoot.transform, false);

			var gameVersionTxt = window.transform.BFSearch("GameVersion").GetComponent<Text>();
			var resVersionTxt = window.transform.BFSearch("ResVersion").GetComponent<Text>();
			if (MotionEngine.Contains(typeof(PatchManager)))
			{
				gameVersionTxt.text = $"Version : {PatchManager.Instance.GetRequestedGameVersion()}";
				resVersionTxt.text = $"Res : {PatchManager.Instance.GetRequestedResourceVersion()}";
			}
			else
			{
				gameVersionTxt.text = "NO Server";
				resVersionTxt.text = string.Empty;
			}
		}

		// 加载背景图片
		GameLog.Log("Load Texture");
		{		
			var handle1 = ResourceManager.Instance.LoadAssetAsync<Texture>("UITexture/Background/bg.png");
			yield return handle1;
			var bg1 = window.transform.BFSearch("Bg1").GetComponent<RawImage>();
			bg1.texture = handle1.AssetObject as Texture;

			var handle2 = ResourceManager.Instance.LoadAssetAsync<Texture>("UITexture/Background/bg.jpg");
			yield return handle2;
			var bg2 = window.transform.BFSearch("Bg2").GetComponent<RawImage>();
			bg2.texture = handle2.AssetObject as Texture;
		}

		// 加载精灵图片
		{
			GameLog.Log("Load Sprite");

			// 设置精灵1
			var handle1 = ResourceManager.Instance.LoadAssetAsync<Sprite>("UITexture/Foods/eggs");
			yield return handle1;
			Image img1 = window.transform.BFSearch("FoodImg1").GetComponent<Image>();
			img1.sprite = handle1.AssetObject as Sprite;
			img1.SetNativeSize();

			// 设置精灵2
			var handle2 = ResourceManager.Instance.LoadAssetAsync<Sprite>("UITexture/Foods/apple");
			yield return handle2;
			Image img2 = window.transform.BFSearch("FoodImg2").GetComponent<Image>();
			img2.sprite = handle2.AssetObject as Sprite;
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

		// 加载LUA文件
		GameLog.Log("Load LUA");
		{
			var handle1 = ResourceManager.Instance.LoadAssetSync<TextAsset>("Lua/LuaTest1.lua");
			TextAsset lua1 = handle1.AssetObject as TextAsset;
			Debug.Log(lua1.text);

			var handle2 = ResourceManager.Instance.LoadAssetSync<TextAsset>("Lua/LuaTest2.lua.txt");
			TextAsset lua2 = handle2.AssetObject as TextAsset;
			Debug.Log(lua2.text);
		}
	}
}