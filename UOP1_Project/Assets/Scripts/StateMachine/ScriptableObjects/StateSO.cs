using UnityEngine;

namespace UOP1.StateMachine.ScriptableObjects
{
	[CreateAssetMenu(fileName = "New State", menuName = "State Machines/State")]
	public class StateSO : ScriptableObject
	{
		[SerializeField] private StateActionSO[] _actions = null;
		[SerializeField] private StateTransitionSO[] _transitions = null;

		/// <summary>
		/// Will create a new state or return an existing one inside the <paramref name="stateMachine"/>.
		/// <para>Will automatically create or reference an existing instance of every Action, Transition, Condition and subsequent states for this <paramref name="stateMachine"/>.</para>
		/// </summary>
		internal State GetState(StateMachine stateMachine)
		{
			if (stateMachine.createdInstances.TryGetValue(this, out var obj))
				return (State)obj;

			var state = new State();
			stateMachine.createdInstances.Add(this, state);

			state._originSO = this;
			state._stateMachine = stateMachine;
			state._transitions = GetTransitions(_transitions, stateMachine);
			state._actions = GetActions(_actions, stateMachine);

			return state;
		}

		private static StateTransition[] GetTransitions(StateTransitionSO[] scriptableTransitions,
			StateMachine stateMachine)
		{
			int count = scriptableTransitions.Length;
			var transitions = new StateTransition[count];
			for (int i = 0; i < count; i++)
				transitions[i] = scriptableTransitions[i].GetTransition(stateMachine);

			return transitions;
		}

		private static StateAction[] GetActions(StateActionSO[] scriptableActions,
			StateMachine stateMachine)
		{
			int count = scriptableActions.Length;
			var actions = new StateAction[count];
			for (int i = 0; i < count; i++)
				actions[i] = scriptableActions[i].GetAction(stateMachine);

			return actions;
		}
	}
}
