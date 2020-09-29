namespace UOP1.FSM35
{
	public delegate void StateAction();

	class ActionKey
	{
		public int SourceStateID;
		public int SourceTransitionID;
	}

	public class FSMAction<TState, TTransition>
	{
		private FSMState<TState> initialState;
		private FSMTransition<TTransition> transition;
		private FSMState<TState> finalState;
		private StateTransitionType transitionType;

		public FSMAction(FSMState<TState> initialState, 
			FSMTransition<TTransition> fsmTransition, 
			FSMState<TState> finalState, 
			StateTransitionType transitionType)
		{
			this.initialState = initialState;
			transition = fsmTransition;
			this.finalState = finalState;
			this.transitionType = transitionType;
		}

		public override string ToString()
		{
			return $"{initialState} to {finalState}";
		}

		public StateTransitionType StateTransitionType()
		{
			return transitionType;
		}

		public FSMState<TState> InitialState => initialState;

		public FSMTransition<TTransition> Transition => transition;

		public FSMState<TState> FinalState => finalState;
	}
}