using UOP1.StateMachine.ScriptableObjects;

namespace UOP1.StateMachine
{
	/// <summary>
	/// Class that represents a conditional statement.
	/// </summary>
	public abstract class Condition : IStateComponent
	{
		private bool _isCached = false;
		private bool _cachedStatement = default;
		internal StateConditionSO _originSO;

		/// <summary>
		/// Use this property to access shared data from the <see cref="StateConditionSO"/> that corresponds to this <see cref="Condition"/>
		/// </summary>
		protected StateConditionSO OriginSO => _originSO;

		/// <summary>
		/// Specify the statement to evaluate.
		/// </summary>
		/// <returns></returns>
		protected abstract bool Statement();

		/// <summary>
		/// Wrap the <see cref="Statement"/> so it can be cached.
		/// </summary>
		internal bool GetStatement()
		{
			if (!_isCached)
			{
				_isCached = true;
				_cachedStatement = Statement();
			}

			return _cachedStatement;
		}

		internal void ClearStatementCache()
		{
			_isCached = false;
		}

		/// <summary>
		/// Awake is called when creating a new instance. Use this method to cache the components needed for the condition.
		/// </summary>
		/// <param name="stateMachine">The <see cref="StateMachine"/> this instance belongs to.</param>
		public virtual void Awake(StateMachine stateMachine) { }
		public virtual void OnStateEnter() { }
		public virtual void OnStateExit() { }
	}

	/// <summary>
	/// Struct containing a Condition and its expected result.
	/// </summary>
	public readonly struct StateCondition
	{
		internal readonly StateMachine _stateMachine;
		internal readonly Condition _condition;
		internal readonly bool _expectedResult;

		public StateCondition(StateMachine stateMachine, Condition condition, bool expectedResult)
		{
			_stateMachine = stateMachine;
			_condition = condition;
			_expectedResult = expectedResult;
		}

		public bool IsMet()
		{
			bool statement = _condition.GetStatement();
			bool isMet = statement == _expectedResult;

#if UNITY_EDITOR
			_stateMachine._debugger.TransitionConditionResult(_condition._originSO.name, statement, isMet);
#endif
			return isMet;
		}
	}
}
