namespace DeivSky.StateMachine
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
		/// <remarks>To allow overriding any default behaviour set internally or through the inspector, <see cref="StateMachine.TryGetScriptableObject{T}(out T)"/> can be used, overriding if T was found.
		/// Make sure to specify the expected type of T and what exactly would get overrriden by it.</remarks>
		public virtual void Awake(StateMachine stateMachine) { }

		public virtual void OnStateEnter() { }
		public virtual void OnStateExit() { }
	}
}
