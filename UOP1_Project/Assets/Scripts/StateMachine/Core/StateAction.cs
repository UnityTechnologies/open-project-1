namespace DeivSky.StateMachine
{
    /// <summary>
    /// An object representing an action.
    /// </summary>
    public abstract class StateAction : IStateComponent
    {
        /// <summary>
        /// Perform the action this object represents.
        /// </summary>
        public abstract void Perform();

        /// <summary>
        /// Awake is called when creating a new instance. Use this method to cache the components needed for the action.
        /// </summary>
        /// <param name="stateMachine">The state machine this instance belongs to.</param>
        public virtual void Awake(StateMachine stateMachine) { }

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
    }
}
