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
		private Image _animImg;
		private RectTransform _animRectTrans;
		private bool _isPlayOpenAnimation = false;

		public override void OnCreate()
		{
			_animRectTrans = GetUIElement("UIMap/Target") as RectTransform;
			_animRectTrans.transform.localScale = Vector3.zero;
			_animImg = GetUIComponent<Image>("UIMap/Target");

			// 监听按钮点击事件
			AddButtonListener("UIMap/Mask", OnClickMask);
			AddButtonListener("UIMap/Target/Shake", OnClickShake);
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
				ITweenNode tween = ParallelNode.Allocate(
					_animRectTrans.transform.TweenScaleTo(0.8f, Vector3.one).SetEase(TweenEase.Bounce.EaseOut),
					_animRectTrans.transform.TweenAnglesTo(0.4f, new Vector3(0, 0, 720))
					);
				TweenGrouper.Play(tween);
			}

			// 闪烁动画
			TweenGrouper.Play(_animImg.TweenColor(0.5f, Color.green, Color.red).SetLoop(ETweenLoop.PingPong));
		}
		public override void OnUpdate()
		{
		}

		private void OnClickMask()
		{
			// 窗口关闭动画
			ITweenNode tween = SequenceNode.Allocate(
				_animRectTrans.TweenAnchoredPositionTo(0.5f, new Vector2(800, 0)).SetLerp(LerpBezierFun),
				_animRectTrans.transform.TweenScaleTo(0.5f, Vector3.zero).SetEase(TweenEase.Bounce.EaseOut),
				ExecuteNode.Allocate(() => { UITools.CloseWindow<UIMap>(); })
				);
			TweenGrouper.Play(tween);
		}
		private void OnClickShake()
		{
			var desktop = WindowManager.Instance.Root.UIDesktop;
			var tween = desktop.transform.ShakePosition(2f, new Vector3(10, 10, 0)).SetEase(TweenEase.Quad.EaseInOut);
			tween.SetDispose(() => { desktop.transform.position = Vector3.zero; });  //注意：震动节点会在面板销毁的时候一起移除，所以需要归位
			TweenGrouper.Play(tween);
		}

		// 贝塞尔
		private Vector2 LerpBezierFun(Vector2 from, Vector2 to, float progress)
		{
			Vector3 control = Vector3.one * 500;
			return TweenMath.QuadBezier(from, control, to, progress);
		}
	}
}