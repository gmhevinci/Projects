using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Patch;
using MotionFramework.Scene;

public class LobbyScene : MonoBehaviour
{
	private AssetOperationHandle _lobbyWindowHandle;
	private AssetOperationHandle _lobbyBg1Handle;
	private AssetOperationHandle _lobbyBg2Handle;
	private AssetOperationHandle _lua1Handle;
	private AssetOperationHandle _lua2Handle;

	private Button _downloadBtn;
	private Text _downloadTips;
	private PatchDownloader _downloader;


	private void Start()
	{
		GameObject uiRoot = GameObject.Find("UIRoot");

		GameLog.Log("加载大厅窗口");
		_lobbyWindowHandle = ResourceManager.Instance.LoadAssetSync<GameObject>("UIPanel/LobbyWindow");
		GameObject window = _lobbyWindowHandle.InstantiateObject;
		window.transform.SetParent(uiRoot.transform, false);

		// 关卡按钮
		var level1Btn = window.transform.BFSearch("Level1Button").GetComponent<Button>();
		level1Btn.onClick.AddListener(OnClickLevel1);
		var level2Btn = window.transform.BFSearch("Level2Button").GetComponent<Button>();
		level2Btn.onClick.AddListener(OnClickLevel2);
		var level3Btn = window.transform.BFSearch("Level3Button").GetComponent<Button>();
		level3Btn.onClick.AddListener(OnClickLevel3);

		// 下载按钮
		PatchDownloader downloader = PatchManager.Instance.CreateDLCDownloader("level3", 1, 1);
		_downloadBtn = window.transform.BFSearch("DownloadBtn").GetComponent<Button>();
		_downloadTips = window.transform.BFSearch("DownloadTips").GetComponent<Text>();
		if(downloader.TotalDownloadBytes == 0)
		{
			_downloadBtn.gameObject.SetActive(false);
		}
		else
		{
			_downloadBtn.onClick.AddListener(OnClickDownload);
		}

		GameLog.Log("加载LUA文件");
		{
			_lua1Handle = ResourceManager.Instance.LoadAssetSync<TextAsset>("Lua/LuaTest1.lua");
			TextAsset lua1 = _lua1Handle.AssetObject as TextAsset;
			GameLog.Log(lua1.text);

			_lua2Handle = ResourceManager.Instance.LoadAssetSync<TextAsset>("Lua/LuaTest2.lua.txt");
			TextAsset lua2 = _lua2Handle.AssetObject as TextAsset;
			GameLog.Log(lua2.text);
		}
	}
	private void Update()
	{
		if (_downloader != null)
			_downloader.Update();
	}
	private void OnDestroy()
	{
		_lobbyWindowHandle.Release();
		_lobbyBg1Handle.Release();
		_lobbyBg2Handle.Release();
		_lua1Handle.Release();
		_lua2Handle.Release();

		if(_downloader != null)
		{
			_downloader.Forbid();
			_downloader = null;
		}
	}

	private void OnClickLevel1()
	{
		PlayGame(1);
	}
	private void OnClickLevel2()
	{
		PlayGame(2);
	}
	private void OnClickLevel3()
	{
		PlayGame(3);
	}
	private void OnClickDownload()
	{
		if(_downloader == null)
		{
			_downloader = PatchManager.Instance.CreateDLCDownloader("level3", 3, 3);
			_downloader.OnDownloadProgressCallback = OnDownloadProgress;
			_downloader.OnDownloadOverCallback = OnDownloadOver;
			_downloader.Download();
		}
	}
	private void OnDownloadProgress(int totalDownloadCount, int currentDownloadCoun, long totalDownloadBytes, long currentDownloadBytes)
	{
		_downloadTips.text = $"正在下载 ({(float)currentDownloadBytes/totalDownloadBytes}%)";
	}
	private void OnDownloadOver(bool succeed)
	{
		if(succeed)
		{
			_downloadBtn.gameObject.SetActive(false);
		}
	}

	private void PlayGame(int level)
	{
		Demo.Instance.PlayLevel = level;
		SceneManager.Instance.ChangeMainScene("Scene/Game", true, null);
	}
}
