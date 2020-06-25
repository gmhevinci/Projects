using UnityEngine;
using MotionFramework.AI;

namespace Hotfix
{
	public class FsmPlay : IFsmNode
	{
		public string Name { get { return "FsmPlay"; } }

		public void OnEnter()
		{
			HotfixLogger.Log("进入FsmPlay");
		}
		public void OnUpdate()
		{
		}
		public void OnExit()
		{
		}

		public void OnHandleMessage(object msg)
		{
		}
	}
}