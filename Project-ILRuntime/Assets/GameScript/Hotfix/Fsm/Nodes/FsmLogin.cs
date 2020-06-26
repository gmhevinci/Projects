using UnityEngine;
using MotionFramework.AI;

namespace Hotfix
{
	public class FsmLogin : IFsmNode
	{
		public string Name { get { return "FsmLogin"; } }

		public void OnEnter()
		{
			HotfixLog.Log("进入FsmLogin");
		}
		public void OnUpdate()
		{
			FsmManager.Instance.Change("FsmPlay");
		}
		public void OnExit()
		{
		}

		public void OnHandleMessage(object msg)
		{
		}
	}
}