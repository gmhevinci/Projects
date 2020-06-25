using System.Collections.Generic;
using UnityEngine;
using MotionFramework;
using MotionFramework.AI;

namespace Hotfix
{
	public class FsmInit : IFsmNode
	{
		public string Name { get { return "FsmInit"; } }

		public void OnEnter()
		{
			HotfixLogger.Log("进入FsmInit");
		}
		public void OnUpdate()
		{
			FsmManager.Instance.Change("FsmLogin");
		}
		public void OnExit()
		{
		}
		public void OnHandleMessage(object msg)
		{
		}
	}
}