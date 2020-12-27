using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework;
using MotionFramework.Patch;
using MotionFramework.Resource;
using MotionFramework.Event;

public class PatchWindow
{
	public static readonly PatchWindow Instance = new PatchWindow();

	/// <summary>
	/// 对话框封装类
	/// </summary>
	private class MessageBox
	{
		private GameObject _cloneObject;
		private Text _content;
		private System.Action _clickYes;

		public void Create(GameObject cloneObject)
		{
			_cloneObject = cloneObject;
			_content = cloneObject.transform.BFSearch("txt_content").GetComponent<Text>();
			var btnYes = cloneObject.transform.BFSearch("btn_yes").GetComponent<Button>();
			btnYes.onClick.AddListener(OnClickYes);
		}
		public void Show(string content, System.Action clickYes)
		{
			_content.text = content;
			_clickYes = clickYes;
			_cloneObject.SetActive(true);
			_cloneObject.transform.SetAsLastSibling();
		}
		public void Hide()
		{
			_content.text = string.Empty;
			_clickYes = null;
			_cloneObject.SetActive(false);
		}
		public bool ActiveSelf
		{
			get
			{
				return _cloneObject.activeSelf;
			}
		}

		private void OnClickYes()
		{
			_clickYes?.Invoke();
			Hide();
		}
	}

	private AssetOperationHandle _handle;
	private EventGroup _eventGroup = new EventGroup();
	private List<MessageBox> _msgBoxList = new List<MessageBox>();

	// UGUI相关
	private GameObject _uiRoot;
	private UIManifest _manifest;
	private GameObject _messageBoxObj;
	private Slider _slider;
	private Text _tips;


	/// <summary>
	/// 异步初始化
	/// </summary>
	/// <returns></returns>
	public IEnumerator InitializeAsync()
	{
		// 下载面板
		string location = "UIPanel/PatchWindow";
		_handle = ResourceManager.Instance.LoadAssetAsync<GameObject>(location);
		yield return _handle;

		if(_handle.AssetObject == null)
			throw new Exception("PatchWindow load failed.");

		_uiRoot = _handle.InstantiateObject;
		_manifest = _uiRoot.GetComponent<UIManifest>();
		_slider = _manifest.GetUIComponent<Slider>("PatchWindow/UIWindow/Slider");
		_tips = _manifest.GetUIComponent<Text>("PatchWindow/UIWindow/Slider/txt_tips");
		_tips.text = "正在准备游戏世界......";
		_messageBoxObj = _manifest.GetUIElement("PatchWindow/UIWindow/MessgeBox").gameObject;
		_messageBoxObj.SetActive(false);

		_eventGroup.AddListener<PatchEventMessageDefine.PatchStatesChange>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.FoundForceInstallAPP>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.FoundUpdateFiles>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.DownloadFilesProgress>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.GameVersionRequestFailed>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.WebPatchManifestDownloadFailed>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.WebFileDownloadFailed>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.WebFileCheckFailed>(OnHandleEvent);
	}

	/// <summary>
	/// 销毁
	/// </summary>
	public void Destroy()
	{
		_eventGroup.RemoveAllListener();

		if (_uiRoot != null)
		{
			GameObject.Destroy(_uiRoot);
			_uiRoot = null;
		}

		_handle.Release();
	}

	/// <summary>
	/// 接收事件
	/// </summary>
	private void OnHandleEvent(IEventMessage msg)
	{
		if (msg is PatchEventMessageDefine.PatchStatesChange)
		{
			var message = msg as PatchEventMessageDefine.PatchStatesChange;
			if (message.CurrentStates == EPatchStates.RequestGameVersion)
				_tips.text = "正在请求最新游戏版本";
			else if (message.CurrentStates == EPatchStates.GetWebPatchManifest)
				_tips.text = "正在分析新清单";
			else if (message.CurrentStates == EPatchStates.GetDonwloadList)
				_tips.text = "正在准备下载列表";
			else if (message.CurrentStates == EPatchStates.DownloadWebFiles)
				_tips.text = "正在下载更新文件";
			else if (message.CurrentStates == EPatchStates.DownloadOver)
				_tips.text = "下载更新文件完毕";
			else if (message.CurrentStates == EPatchStates.PatchDone)
				_tips.text = "欢迎来到游戏世界";
		}

		else if (msg is PatchEventMessageDefine.FoundForceInstallAPP)
		{
			var message = msg as PatchEventMessageDefine.FoundForceInstallAPP;
			System.Action callback = () =>
			{
				Application.OpenURL(message.InstallURL);
			};
			ShowMessageBox($"发现强制更新安装包 : {message.NewVersion}，请重新下载游戏", callback);
		}

		else if (msg is PatchEventMessageDefine.FoundUpdateFiles)
		{
			var message = msg as PatchEventMessageDefine.FoundUpdateFiles;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.BeginingDownloadWebFiles);
			};
			float sizeMB = message.TotalSizeBytes / 1048576f;
			sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
			string totalSizeMB = sizeMB.ToString("f1");
			ShowMessageBox($"发现新版本需要更新 : 一共{message.TotalCount}个文件，总大小{totalSizeMB}MB", callback);
		}

		else if (msg is PatchEventMessageDefine.DownloadFilesProgress)
		{
			var message = msg as PatchEventMessageDefine.DownloadFilesProgress;
			_slider.value = message.CurrentDownloadCount / message.TotalDownloadCount;
			string currentSizeMB = (message.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
			string totalSizeMB = (message.TotalDownloadSizeBytes / 1048576f).ToString("f1");
			_tips.text = $"{message.CurrentDownloadCount}/{message.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
		}

		else if (msg is PatchEventMessageDefine.GameVersionRequestFailed)
		{
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryRequestGameVersion);
			};
			ShowMessageBox($"请求最新版本失败，请检查网络状况", callback);
		}

		else if (msg is PatchEventMessageDefine.WebFileDownloadFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileDownloadFailed;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryDownloadWebFiles);
			};
			ShowMessageBox($"文件下载失败 : {message.Name}", callback);
		}

		else if (msg is PatchEventMessageDefine.WebFileCheckFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileCheckFailed;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryDownloadWebFiles);
			};
			ShowMessageBox($"文件验证失败 : {message.Name}", callback);
		}

		else if (msg is PatchEventMessageDefine.WebPatchManifestDownloadFailed)
		{
			var message = msg as PatchEventMessageDefine.WebPatchManifestDownloadFailed;
			System.Action callback = () =>
			{
				SendOperationEvent(EPatchOperation.TryDownloadWebPatchManifest);
			};
			ShowMessageBox($"清单下载失败", callback);
		}

		else
		{
			throw new System.NotImplementedException($"{msg.GetType()}");
		}
	}

	/// <summary>
	/// 显示对话框
	/// </summary>
	private void ShowMessageBox(string content, System.Action clickCallback)
	{
		// 尝试获取一个可用的对话框
		MessageBox msgBox = null;
		for (int i = 0; i < _msgBoxList.Count; i++)
		{
			var item = _msgBoxList[i];
			if (item.ActiveSelf == false)
			{
				msgBox = item;
				break;
			}
		}

		// 如果没有可用的对话框，则创建一个新的对话框
		if (msgBox == null)
		{
			msgBox = new MessageBox();
			var cloneObject = GameObject.Instantiate(_messageBoxObj, _messageBoxObj.transform.parent);
			msgBox.Create(cloneObject);
			_msgBoxList.Add(msgBox);
		}

		// 显示对话框
		msgBox.Show(content, clickCallback);
	}

	/// <summary>
	/// 发送事件
	/// </summary>
	private void SendOperationEvent(EPatchOperation operation)
	{
		PatchEventMessageDefine.OperationEvent msg = new PatchEventMessageDefine.OperationEvent();
		msg.operation = operation;
		EventManager.Instance.SendMessage(msg);
	}
}