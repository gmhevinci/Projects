using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Patch;
using MotionFramework.Scene;

public class LoginScene : MonoBehaviour
{
	private AssetOperationHandle _loginWindowHandle;

	private void Start()
	{
		GameObject uiRoot = GameObject.Find("UIRoot");

		GameLog.Log("加载登录窗口");
		_loginWindowHandle = ResourceManager.Instance.LoadAssetSync<GameObject>("UIPanel/LoginWindow");
		GameObject window = _loginWindowHandle.InstantiateObject;
		window.transform.SetParent(uiRoot.transform, false);

		// 显示版本信息
		var gameVersionTxt = window.transform.BFSearch("GameVersion").GetComponent<Text>();
		var resVersionTxt = window.transform.BFSearch("ResVersion").GetComponent<Text>();
		if (MotionEngine.Contains(typeof(PatchManager)))
		{
			gameVersionTxt.text = $"Game Version : {PatchManager.Instance.GetRequestedGameVersion()}";
			resVersionTxt.text = $"Resource Version : {PatchManager.Instance.GetRequestedResourceVersion()}";
		}
		else
		{
			gameVersionTxt.text = "NO Server";
			resVersionTxt.text = string.Empty;
		}

		// 登录按钮
		var loginBtn = window.transform.BFSearch("LoginButton").GetComponent<Button>();
		loginBtn.onClick.AddListener(OnClickLogin);
	}
	private void OnDestroy()
	{
		_loginWindowHandle.Release();
	}

	private void OnClickLogin()
	{
		SceneManager.Instance.ChangeMainScene("Scene/Lobby", true, null);
	}
}
