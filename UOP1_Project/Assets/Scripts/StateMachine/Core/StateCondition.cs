namespace DeivSky.StateMachine
{
	/// <summary>
	/// Class that represents a conditional statement.
	/// </summary>
	public abstract class Condition : IStateComponent
	{
		/// <summary>
		/// Specify the statement to evaluate.
		/// </summary>
		/// <returns></returns>
		public abstract bool Statement();

		/// <summary>
		/// Awake is called when creating a new instance. Use this method to cache the components needed for the condition.
		/// </summary>
		/// <param name="stateMachine">The <see cref="StateMachine"/> this instance belongs to.</param>
		/// <remarks>To allow overriding any default behaviour set internally or through the inspector, <see cref="StateMachine.TryGetScriptableObject{T}(out T)"/> can be used, overriding if T was found.
		/// Make sure to specify the expected type of T and what exactly would get overrriden by it.</remarks>
		public virtual void Awake(StateMachine stateMachine) { }
		public virtual void OnStateEnter() { }
		public virtual void OnStateExit() { }
	}

	/// <summary>
	/// Struct containing a Condition and its expected result.
	/// </summary>
	public readonly struct StateCondition
	{
		internal readonly Condition _condition;
		internal readonly bool _expectedResult;

		public StateCondition(Condition condition, bool expectedResult)
		{
			_condition = condition;
			_expectedResult = expectedResult;
		}

		public bool IsMet => _condition.Statement() == _expectedResult;
	}
}
