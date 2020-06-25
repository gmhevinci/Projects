using MotionFramework.AI;

namespace Hotfix
{
	public class FsmManager
	{
		public readonly static FsmManager Instance = new FsmManager();

		private readonly FiniteStateMachine _fsm = new FiniteStateMachine();

		public void Create()
		{
			_fsm.AddNode(new FsmInit());
			_fsm.AddNode(new FsmLogin());
			_fsm.AddNode(new FsmPlay());
			_fsm.Run("FsmInit");
		}

		public void Update()
		{
			_fsm.Update();
		}

		public void Change(string node)
		{
			_fsm.Transition(node);
		}
	}
}