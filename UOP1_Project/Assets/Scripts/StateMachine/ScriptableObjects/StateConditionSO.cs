using System.Collections.Generic;
using UnityEngine;

namespace UOP1.StateMachine.ScriptableObjects
{
	public abstract class StateConditionSO : ScriptableObject
	{
		internal StateCondition GetCondition(StateMachine stateMachine, bool expectedResult, Dictionary<ScriptableObject, object> createdInstances)
		{
			if (createdInstances.TryGetValue(this, out var cond))
				return new StateCondition((Condition)cond, expectedResult);

			var condition = new StateCondition(CreateCondition(), expectedResult);
			createdInstances.Add(this, condition._condition);
			condition._condition.Awake(stateMachine);
			return condition;
		}
		protected abstract Condition CreateCondition();
	}


	public abstract class StateConditionSO<T> : StateConditionSO where T : Condition, new()
	{
		protected override Condition CreateCondition() => new T();
	}
}
