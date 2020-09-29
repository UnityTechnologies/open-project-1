using System.Collections.Generic;
using System.Diagnostics;

namespace UOP1.FSM35
{
	public class FSMState<TState>
	{
		private TState _State;


		public FSMState(TState state)
		{
			_State = state;
			ID = _State.ToString().GetDeterministicHashCode();
			Debug.WriteLine($"{_State}: {ID}");
		}

		public override string ToString()
		{
			return _State.ToString();
		}

		public TState State => _State;

		public int ID { get; } = 0;

		private List<StateAction> _EntryAction = new List<StateAction>();
		public List<StateAction> EntryAction
		{
			get { return _EntryAction; }
		}
		public void AddEntryAction(StateAction action)
		{
			_EntryAction.Add(action);
		}

		private List<StateAction> _ExitAction = new List<StateAction>();
		public List<StateAction> ExitAction
		{
			get { return _ExitAction; }
		}
		public void AddExitAction(StateAction action)
		{
			_ExitAction.Add(action);
		}
	}
}