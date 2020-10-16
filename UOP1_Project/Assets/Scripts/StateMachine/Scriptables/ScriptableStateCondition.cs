using System.Collections.Generic;
using UnityEngine;

namespace DeivSky.StateMachine.Scriptables
{
	public abstract class ScriptableStateCondition : ScriptableObject
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


	public abstract class ScriptableStateCondition<T> : ScriptableStateCondition where T : Condition, new()
	{
		protected override Condition CreateCondition() => new T();
	}

	public abstract class SerializableStateCondition<T> : ScriptableStateCondition where T : Condition, new()
	{
		[SerializeField] private T _condition = new T();
		protected override Condition CreateCondition() => _condition;
	}
}
