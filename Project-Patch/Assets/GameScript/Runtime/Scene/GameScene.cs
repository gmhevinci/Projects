using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework;
using MotionFramework.Resource;
using MotionFramework.Patch;
using MotionFramework.Scene;

public class GameScene : MonoBehaviour
{
	private AssetOperationHandle _gameWindowHandle;
	private AssetOperationHandle _photoHandle;
	private AssetOperationHandle _monsterHandle;
	private GameObject _window;

	private void Start()
	{
		GameObject uiRoot = GameObject.Find("UIRoot");

		// 加载窗口
		GameLog.Log("加载游戏窗口");
		_gameWindowHandle = ResourceManager.Instance.LoadAssetSync<GameObject>("UIPanel/GameWindow");
		_window = _gameWindowHandle.InstantiateObject;
		_window.transform.SetParent(uiRoot.transform, false);

		// 退出按钮
		var exitBtn = _window.transform.BFSearch("Exit").GetComponent<Button>();
		exitBtn.onClick.AddListener(OnClickExit);

		// 加载模型
		{
			GameLog.Log("加载模型");
			if (Demo.Instance.PlayLevel == 1)
			{
				_monsterHandle = ResourceManager.Instance.LoadAssetAsync<GameObject>("Entity/Level1/footman_Blue");
				_monsterHandle.Completed += MonsterHandle_Completed;
			}
			else if (Demo.Instance.PlayLevel == 2)
			{
				_monsterHandle = ResourceManager.Instance.LoadAssetAsync<GameObject>("Entity/Level2/footman_Green");
				_monsterHandle.Completed += MonsterHandle_Completed;
			}
			else if (Demo.Instance.PlayLevel == 3)
			{
				_monsterHandle = ResourceManager.Instance.LoadAssetAsync<GameObject>("Entity/Level3/footman_Red");
				_monsterHandle.Completed += MonsterHandle_Completed;
			}
		}

		// 加载头像
		this.StartCoroutine(LoadPhotoAsync());
	}
	private void OnDestroy()
	{
		_gameWindowHandle.Release();
		_photoHandle.Release();
		_monsterHandle.Release();
	}

	private void OnClickExit()
	{
		SceneManager.Instance.ChangeMainScene("Scene/Lobby", true, null);
	}
	private IEnumerator LoadPhotoAsync()
	{
		GameLog.Log("加载头像");
		if (Demo.Instance.PlayLevel == 1)
		{
			_photoHandle = ResourceManager.Instance.LoadAssetAsync<Sprite>("UITexture/Photos/eggs");
		}
		else if (Demo.Instance.PlayLevel == 2)
		{
			_photoHandle = ResourceManager.Instance.LoadAssetAsync<Sprite>("UITexture/Photos/apple");
		}
		else if (Demo.Instance.PlayLevel == 3)
		{
			_photoHandle = ResourceManager.Instance.LoadAssetAsync<Sprite>("UITexture/Photos/magic_fish");
		}

		yield return _photoHandle;
		Image img = _window.transform.BFSearch("Photo").GetComponent<Image>();
		img.sprite = _photoHandle.AssetObject as Sprite;
	}
	private void MonsterHandle_Completed(AssetOperationHandle handle)
	{
		var monster = handle.InstantiateObject;
		monster.transform.position = Vector3.zero;
		monster.transform.localScale = monster.transform.localScale * 2f;
	}
}