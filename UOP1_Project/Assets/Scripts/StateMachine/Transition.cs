namespace KarimCastagnini.PluggableFSM
{
	public class Transition<T>
	{
		public State<T> NextState { get; private set; }

		private Condition<T> _condition;
		private bool _conditionResult;

		//conditionResult specifies if the condition must be True of False in order to be met
		public Transition(State<T> toState, Condition<T> condition, bool conditionResult)
		{
			NextState = toState;
			_condition = condition;
			_conditionResult = conditionResult;
		}

		public bool ConditionIsMet(T dataModel)
			=> _conditionResult ? _condition.IsMet(dataModel) : !_condition.IsMet(dataModel);
	}
}
