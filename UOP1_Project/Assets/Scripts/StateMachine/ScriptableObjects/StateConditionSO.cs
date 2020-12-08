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
