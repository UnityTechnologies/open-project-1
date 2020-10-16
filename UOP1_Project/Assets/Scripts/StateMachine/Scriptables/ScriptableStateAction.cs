using System.Collections.Generic;
using UnityEngine;

namespace DeivSky.StateMachine.Scriptables
{
	public abstract class ScriptableStateAction : ScriptableObject
	{
		internal StateAction GetAction(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
		{
			if (createdInstances.TryGetValue(this, out var obj))
				return (StateAction)obj;

			var action = CreateAction();
			createdInstances.Add(this, action);
			action.Awake(stateMachine);
			return action;
		}
		protected abstract StateAction CreateAction();
	}

	public abstract class ScriptableStateAction<T> : ScriptableStateAction where T : StateAction, new()
	{
		protected override StateAction CreateAction() => new T();
	}
}
