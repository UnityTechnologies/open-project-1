using System.Collections.Generic;
using UnityEngine;

namespace UOP1.StateMachine.ScriptableObjects
{
	public abstract class StateConditionSO : ScriptableObject
	{
		/// <summary>
		/// Will create a new custom <see cref="Condition"/> or use an existing one inside <paramref name="createdInstances"/>.
		/// </summary>
		internal StateCondition GetCondition(StateMachine stateMachine, bool expectedResult, Dictionary<ScriptableObject, object> createdInstances)
		{
			if (!createdInstances.TryGetValue(this, out var obj))
			{
				var condition = CreateCondition();
				condition._originSO = this;
				createdInstances.Add(this, condition);
				condition.Awake(stateMachine);

				obj = condition;
			}

			return new StateCondition(stateMachine, (Condition)obj, expectedResult);
		}
		protected abstract Condition CreateCondition();
	}


	public abstract class StateConditionSO<T> : StateConditionSO where T : Condition, new()
	{
		protected override Condition CreateCondition() => new T();
	}
}
