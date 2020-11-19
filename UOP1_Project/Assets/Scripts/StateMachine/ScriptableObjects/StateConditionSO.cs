using System.Collections.Generic;
using UnityEngine;

namespace UOP1.StateMachine.ScriptableObjects
{
	public abstract class StateConditionSO : ScriptableObject
	{
		[SerializeField]
		[Tooltip("The condition will only be evaluated once each frame, and cached for subsequent uses.\r\n\r\nThe caching is unique to each instance of the State Machine and of the Scriptable Object (i.e. Instances of the State Machine or Condition don't share results if they belong to different GameObjects).")]
		internal bool cacheResult = true;

		/// <summary>
		/// Will create a new custom <see cref="Condition"/> or use an existing one inside <paramref name="createdInstances"/>.
		/// </summary>
		internal StateCondition GetCondition(StateMachine stateMachine, bool expectedResult, Dictionary<ScriptableObject, object> createdInstances)
		{
			if (createdInstances.TryGetValue(this, out var cond))
				return new StateCondition(this, stateMachine, (Condition)cond, expectedResult);

			var condition = new StateCondition(this, stateMachine, CreateCondition(), expectedResult);
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
