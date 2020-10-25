namespace UOP1.StateMachine
{
	/// <summary>
	/// An object representing an action.
	/// </summary>
	public abstract class StateAction : IStateComponent
	{
		/// <summary>
		/// Called every frame the <see cref="StateMachine"/> is in a <see cref="State"/> with this <see cref="StateAction"/>.
		/// </summary>
		public abstract void OnUpdate();

		/// <summary>
		/// Awake is called when creating a new instance. Use this method to cache the components needed for the action.
		/// </summary>
		/// <param name="stateMachine">The <see cref="StateMachine"/> this instance belongs to.</param>
		public virtual void Awake(StateMachine stateMachine) { }

		public virtual void OnStateEnter() { }
		public virtual void OnStateExit() { }
	}
}
