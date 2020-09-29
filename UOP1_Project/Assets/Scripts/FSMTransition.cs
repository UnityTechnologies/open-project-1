namespace UOP1.FSM35
{
	public class FSMTransition<TTransition>
	{
		private int id = 0;
		private TTransition transition;

		public FSMTransition(TTransition myTransition)
		{
			transition = myTransition;
			id = transition.ToString().GetDeterministicHashCode();
		}

		public override string ToString()
		{
			return transition.ToString();
		}

		public TTransition Transition => transition;

		public int ID => id;
	}
}