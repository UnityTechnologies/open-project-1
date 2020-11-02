using UnityEngine;

namespace UOP1.StateMachine.ScriptableObjects
{
	public abstract class StateActionSO : ScriptableObject
	{
		/// <summary>
		/// Will create a new custom <see cref="StateAction"/> or return an existing one inside the <paramref name="stateMachine"/>
		/// </summary>
		internal StateAction GetAction(StateMachine stateMachine)
		{
			if (stateMachine.createdInstances.TryGetValue(this, out var obj))
				return (StateAction)obj;

			var action = CreateAction();
			stateMachine.createdInstances.Add(this, action);
			action._originSO = this;
			action.Awake(stateMachine);
			return action;
		}
		protected abstract StateAction CreateAction();
	}

	public abstract class StateActionSO<T> : StateActionSO where T : StateAction, new()
	{
		protected override StateAction CreateAction() => new T();
	}
}
