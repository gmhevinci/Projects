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
		private Button _btnYes;
		private Button _btnNo;
		private System.Action _clickYes;
		private System.Action _clickNo;

		public bool ActiveSelf
		{
			get
			{
				return _cloneObject.activeSelf;
			}
		}

		public void Create(GameObject cloneObject)
		{
			_cloneObject = cloneObject;
			_content = cloneObject.transform.BFSearch("txt_content").GetComponent<Text>();
			_btnYes = cloneObject.transform.BFSearch("btn_yes").GetComponent<Button>();
			_btnYes.onClick.AddListener(OnClickYes);
			_btnNo = cloneObject.transform.BFSearch("btn_no").GetComponent<Button>();
			_btnNo.onClick.AddListener(OnClickNo);
		}
		public void Show(string content, System.Action clickYes)
		{
			_content.text = content;
			var rectTrans = _btnYes.transform as RectTransform;
			rectTrans.anchoredPosition = new Vector2(0, -126);
			_btnNo.gameObject.SetActive(false);
			_clickYes = clickYes;
			_clickNo = null;
			_cloneObject.SetActive(true);
			_cloneObject.transform.SetAsLastSibling();
		}
		public void Show(string content, System.Action clickYes, System.Action clickNo)
		{
			_content.text = content;
			var rectTrans = _btnYes.transform as RectTransform;
			rectTrans.anchoredPosition = new Vector2(-178, -126);
			_btnNo.gameObject.SetActive(true);
			_clickYes = clickYes;
			_clickNo = clickNo;
			_cloneObject.SetActive(true);
			_cloneObject.transform.SetAsLastSibling();
		}
		public void Hide()
		{
			_content.text = string.Empty;
			_clickYes = null;
			_cloneObject.SetActive(false);
		}
		private void OnClickYes()
		{
			_clickYes?.Invoke();
			Hide();
		}
		private void OnClickNo()
		{
			_clickNo?.Invoke();
			Hide();
		}
	}


	private readonly EventGroup _eventGroup = new EventGroup();
	private readonly List<MessageBox> _msgBoxList = new List<MessageBox>();

	// UGUI相关
	private GameObject _uiRoot;
	private UIManifest _manifest;
	private GameObject _messageBoxYesObj;
	private Slider _slider;
	private Text _tips;


	/// <summary>
	/// 初始化
	/// </summary>
	public void Initialize()
	{
		var prefab = Resources.Load<GameObject>("PatchWindow");
		_uiRoot = GameObject.Instantiate(prefab);

		_manifest = _uiRoot.GetComponent<UIManifest>();
		_slider = _manifest.GetUIComponent<Slider>("PatchWindow/UIWindow/Slider");
		_tips = _manifest.GetUIComponent<Text>("PatchWindow/UIWindow/Slider/txt_tips");
		_tips.text = "正在准备游戏世界......";
		_messageBoxYesObj = _manifest.GetUIElement("PatchWindow/UIWindow/MessgeBox").gameObject;
		_messageBoxYesObj.SetActive(false);

		_eventGroup.AddListener<PatchEventMessageDefine.PatchStatesChange>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.FoundNewApp>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.FoundUpdateFiles>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.DownloadProgressUpdate>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.GameVersionRequestFailed>(OnHandleEvent);
		_eventGroup.AddListener<PatchEventMessageDefine.PatchManifestRequestFailed>(OnHandleEvent);
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
			else if (message.CurrentStates == EPatchStates.RequestPatchManifest)
				_tips.text = "正在下载新的补丁清单";
			else if (message.CurrentStates == EPatchStates.GetDownloadList)
				_tips.text = "正在准备下载列表";
			else if (message.CurrentStates == EPatchStates.DownloadWebFiles)
				_tips.text = "正在下载更新文件";
			else if (message.CurrentStates == EPatchStates.DownloadOver)
				_tips.text = "下载更新文件完毕";
			else if (message.CurrentStates == EPatchStates.PatchDone)
				_tips.text = "欢迎来到游戏世界";
			else
				throw new NotImplementedException(message.CurrentStates.ToString());
		}

		else if (msg is PatchEventMessageDefine.FoundNewApp)
		{
			var message = msg as PatchEventMessageDefine.FoundNewApp;
			System.Action callbackYes = () =>
			{
				Application.OpenURL(message.InstallURL);
			};

			// 注意：如果是强制安装
			if (message.ForceInstall)
			{
				ShowMessageBox($"发现新的安装包 : {message.NewVersion}，请重新下载游戏", callbackYes);
			}
			else
			{
				System.Action callbackNo = () =>
				{
					HandlePatchOperation(EPatchOperation.BeginGetDownloadList);
				};
				ShowMessageBox($"发现新的安装包 : {message.NewVersion}，是否重新下载游戏", callbackYes, callbackNo);
			}
		}

		else if (msg is PatchEventMessageDefine.FoundUpdateFiles)
		{
			var message = msg as PatchEventMessageDefine.FoundUpdateFiles;
			System.Action callback = () =>
			{
				HandlePatchOperation(EPatchOperation.BeginDownloadWebFiles);
			};
			float sizeMB = message.TotalSizeBytes / 1048576f;
			sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
			string totalSizeMB = sizeMB.ToString("f1");
			ShowMessageBox($"发现新版本需要更新 : 一共{message.TotalCount}个文件，总大小{totalSizeMB}MB", callback);
		}

		else if (msg is PatchEventMessageDefine.DownloadProgressUpdate)
		{
			var message = msg as PatchEventMessageDefine.DownloadProgressUpdate;
			_slider.value = (float)message.CurrentDownloadCount / message.TotalDownloadCount;
			string currentSizeMB = (message.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
			string totalSizeMB = (message.TotalDownloadSizeBytes / 1048576f).ToString("f1");
			_tips.text = $"{message.CurrentDownloadCount}/{message.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
		}

		else if (msg is PatchEventMessageDefine.GameVersionRequestFailed)
		{
			System.Action callback = () =>
			{
				HandlePatchOperation(EPatchOperation.TryRequestGameVersion);
			};
			ShowMessageBox($"请求最新游戏版本失败，请检查网络状况", callback);
		}

		else if (msg is PatchEventMessageDefine.PatchManifestRequestFailed)
		{
			System.Action callback = () =>
			{
				HandlePatchOperation(EPatchOperation.TryRequestPatchManifest);
			};
			ShowMessageBox($"请求最新的清单失败，请检查网络状况", callback);
		}

		else if (msg is PatchEventMessageDefine.WebFileDownloadFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileDownloadFailed;
			System.Action callback = () =>
			{
				HandlePatchOperation(EPatchOperation.TryDownloadWebFiles);
			};
			ShowMessageBox($"文件下载失败 : {message.Name}", callback);
		}

		else if (msg is PatchEventMessageDefine.WebFileCheckFailed)
		{
			var message = msg as PatchEventMessageDefine.WebFileCheckFailed;
			System.Action callback = () =>
			{
				HandlePatchOperation(EPatchOperation.TryDownloadWebFiles);
			};
			ShowMessageBox($"文件验证失败 : {message.Name}", callback);
		}

		else
		{
			throw new System.NotImplementedException($"{msg.GetType()}");
		}
	}

	/// <summary>
	/// 显示对话框
	/// </summary>
	private void ShowMessageBox(string content, System.Action yes, System.Action no = null)
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
			var cloneObject = GameObject.Instantiate(_messageBoxYesObj, _messageBoxYesObj.transform.parent);
			msgBox.Create(cloneObject);
			_msgBoxList.Add(msgBox);
		}

		// 显示对话框
		if (no == null)
			msgBox.Show(content, yes);
		else
			msgBox.Show(content, yes, no);
	}

	/// <summary>
	/// 处理请求
	/// </summary>
	private void HandlePatchOperation(EPatchOperation operation)
	{
		PatchManager.Instance.HandleOperation(operation);
	}
}