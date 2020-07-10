using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Event;
using MotionFramework.Window;
using MotionFramework.Tween;

namespace Hotfix
{
	[Window((int)EWindowLayer.Panel, false)]
	sealed class UIMap : CanvasWindow
	{
		private CanvasGroup _canvasGroup;
		private RectTransform _animRectTrans;
		private bool _isPlayOpenAnimation = false;

		public override void OnCreate()
		{
			_animRectTrans = GetUIElement("UIMap/Target") as RectTransform;
			_animRectTrans.transform.localScale = Vector3.zero;
			_canvasGroup = GetUIComponent<CanvasGroup>("UIMap/Target");

			// 监听按钮点击事件
			AddButtonListener("UIMap/Mask", OnClickMask);
		}
		public override void OnDestroy()
		{
		}
		public override void OnRefresh()
		{
			// 窗口打开动画
			if (_isPlayOpenAnimation == false)
			{
				_isPlayOpenAnimation = true;
				ITweenNode rootNode = ParallelNode.Allocate(
					_canvasGroup.TweenAlpha(0.4f, 0f, 1f),
					_animRectTrans.transform.TweenScaleTo(0.8f, Vector3.one).SetEase(TweenEase.Bounce.EaseOut),
					_animRectTrans.transform.TweenAnglesTo(0.4f, new Vector3(0, 0, 720))
					);
				TweenGrouper.AddNode(rootNode);
			}
		}
		public override void OnUpdate()
		{
		}

		private void OnClickMask()
		{
			// 窗口关闭动画
			ITweenNode rootNode = SequenceNode.Allocate(
				_animRectTrans.TweenAnchoredPositionTo(0.5f, new Vector2(800, 0)).SetLerp(LerpFun),
				_animRectTrans.transform.TweenScaleTo(0.5f, Vector3.zero).SetEase(TweenEase.Bounce.EaseOut),
				ExecuteNode.Allocate(() => { UITools.CloseWindow<UIMap>(); })
				);
			TweenGrouper.AddNode(rootNode);
		}

		// 贝塞尔路径
		private Vector2 LerpFun(Vector2 from, Vector2 to, float progress)
		{
			Vector3 control1 = Vector3.one * 500;
			Vector3 control2 = Vector3.one * -500;
			Vector3[] nodes = new Vector3[4] { from, control1, control2, to };

			float t = progress;
			float d = 1f - t;
			return d * d * d * nodes[0] + 3f * d * d * t * nodes[1] + 3f * d * t * t * nodes[2] + t * t * t * nodes[3];
		}
	}
}