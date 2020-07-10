using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Event;
using MotionFramework.Window;

namespace Hotfix
{
	[Window((int)EWindowLayer.Panel, true)]
	sealed class UITown : CanvasWindow
	{
		private UISprite _photo;

		public override void OnCreate()
		{
			_photo = GetUIComponent<UISprite>("UITown/Photo");

			// 监听按钮点击事件
			AddButtonListener("UITown/Skill1", OnClickSkill1);
			AddButtonListener("UITown/Skill2", OnClickSkill2);
			AddButtonListener("UITown/Skill3", OnClickSkill3);
			AddButtonListener("UITown/Skill4", OnClickSkill4);
			AddButtonListener("UITown/Skill5", OnClickSkill5);
			AddButtonListener("UITown/Map", OnClickMap);
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
		}
		private void OnClickSkill2()
		{
			_photo.SpriteName = "Photo2";
		}
		private void OnClickSkill3()
		{
			_photo.SpriteName = "Photo3";
		}
		private void OnClickSkill4()
		{
			_photo.SpriteName = "Photo4";
		}
		private void OnClickSkill5()
		{
			_photo.SpriteName = "Photo5";
		}
		private void OnClickMap()
		{
			UITools.OpenWindow<UIMap>();
		}
	}
}