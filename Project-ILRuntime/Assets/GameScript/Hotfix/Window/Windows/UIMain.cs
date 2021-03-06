﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Event;
using MotionFramework.Window;

namespace Hotfix
{
	[Window((int)EWindowLayer.Panel, true)]
	sealed class UIMain : CanvasWindow
	{
		private UISprite _photo;

		public override void OnCreate()
		{
			_photo = GetUIComponent<UISprite>("UIMain/Photo");

			// 监听按钮点击事件
			AddButtonListener("UIMain/Skill1", OnClickSkill1);
			AddButtonListener("UIMain/Skill2", OnClickSkill2);
			AddButtonListener("UIMain/Skill3", OnClickSkill3);
			AddButtonListener("UIMain/Skill4", OnClickSkill4);
			AddButtonListener("UIMain/Skill5", OnClickSkill5);
			AddButtonListener("UIMain/Map", OnClickMap);
		}
		public override void OnDestroy()
		{
		}
		public override void OnRefresh()
		{
		}
		public override void OnUpdate()
		{
		}

		private void OnClickSkill1()
		{
			_photo.SpriteName = "Photo1";
			HotfixEventManager.Instance.SendMessage(new HotfixEventDefine.ClickSkill1Event());
		}
		private void OnClickSkill2()
		{
			_photo.SpriteName = "Photo2";
			HotfixEventManager.Instance.SendMessage(new HotfixEventDefine.ClickSkill2Event());
		}
		private void OnClickSkill3()
		{
			_photo.SpriteName = "Photo3";
			HotfixEventManager.Instance.SendMessage(new HotfixEventDefine.ClickSkill3Event());
		}
		private void OnClickSkill4()
		{
			_photo.SpriteName = "Photo4";
			HotfixEventManager.Instance.SendMessage(new HotfixEventDefine.ClickSkill4Event());
		}
		private void OnClickSkill5()
		{
			_photo.SpriteName = "Photo5";
			HotfixEventManager.Instance.SendMessage(new HotfixEventDefine.ClickSkill5Event());
		}
		private void OnClickMap()
		{
			UITools.OpenWindow<UIMap>();
		}
	}
}