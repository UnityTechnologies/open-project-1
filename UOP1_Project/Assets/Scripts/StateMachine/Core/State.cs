using UOP1.StateMachine.ScriptableObjects;

namespace UOP1.StateMachine
{
	public class State
	{
		internal StateSO _originSO;
		internal StateMachine _stateMachine;
		internal StateTransition[] _transitions;
		internal StateAction[] _actions;

		internal State() { }

		public State(
			StateSO originSO,
			StateMachine stateMachine,
			StateTransition[] transitions,
			StateAction[] actions)
		{
			_originSO = originSO;
			_stateMachine = stateMachine;
			_transitions = transitions;
			_actions = actions;
		}

		public void OnStateEnter()
		{
			void OnStateEnter(IStateComponent[] comps)
			{
				for (int i = 0; i < comps.Length; i++)
					comps[i].OnStateEnter();
			}
			OnStateEnter(_transitions);
			OnStateEnter(_actions);
		}

		public void OnUpdate()
		{
			for (int i = 0; i < _actions.Length; i++)
				_actions[i].OnUpdate();
		}

		public void OnStateExit()
		{
			void OnStateExit(IStateComponent[] comps)
			{
				for (int i = 0; i < comps.Length; i++)
					comps[i].OnStateExit();
			}
			OnStateExit(_transitions);
			OnStateExit(_actions);
		}

		public bool TryGetTransition(out State state)
		{
			state = null;

			for (int i = 0; i < _transitions.Length; i++)
				if (_transitions[i].TryGetTransiton(out state))
					break;

			for (int i = 0; i < _transitions.Length; i++)
				_transitions[i].ClearConditionsCache();

			return state != null;
		}
	}
}
