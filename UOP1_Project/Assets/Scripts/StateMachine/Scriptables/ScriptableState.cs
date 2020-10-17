using System.Collections.Generic;
using UnityEngine;

namespace DeivSky.StateMachine.Scriptables
{
	[CreateAssetMenu(fileName = "New State", menuName = "State Machines/State")]
	public class ScriptableState : ScriptableObject
	{
		[SerializeField] private ScriptableStateAction[] _actions = null;
		[SerializeField] private ScriptableStateTransition[] _transitions = null;

		public State GetState(StateMachine stateMachine)
			=> GetState(stateMachine, new Dictionary<ScriptableObject, object>());

		internal State GetState(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
		{
			if (createdInstances.TryGetValue(this, out var obj))
				return (State)obj;

			var state = new State();
			createdInstances.Add(this, state);

			state.Name = name;
			state._stateMachine = stateMachine;
			state._transitions = GetTransitions(_transitions, stateMachine, createdInstances);
			state._actions = GetActions(_actions, stateMachine, createdInstances);

			return state;
		}

		private static StateTransition[] GetTransitions(ScriptableStateTransition[] scriptableTransitions,
			StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
		{
			int count = scriptableTransitions.Length;
			var transitions = new StateTransition[count];
			for (int i = 0; i < count; i++)
				transitions[i] = scriptableTransitions[i].GetTransition(stateMachine, createdInstances);

			return transitions;
		}

		private static StateAction[] GetActions(ScriptableStateAction[] scriptableActions,
			StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
		{
			int count = scriptableActions.Length;
			var actions = new StateAction[count];
			for (int i = 0; i < count; i++)
				actions[i] = scriptableActions[i].GetAction(stateMachine, createdInstances);

			return actions;
		}
	}
}
