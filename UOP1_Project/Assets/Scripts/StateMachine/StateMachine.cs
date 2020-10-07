using UnityEngine;

namespace KarimCastagnini.PluggableFSM
{
	public abstract class StateMachine<T> : MonoBehaviour
	{
		private State<T> _currentState;

		private void Awake()
		{
			TransitionTable<T> table = GetTransitionTable();

			if (table == null)
				return; //implement a properly formatted warning message

			_currentState = table.InitialState;
		}

		private void Update()
		{
			T data = GetData();
			TransitionTable<T> table = GetTransitionTable();

			if (data == null || table == null)
				return; //implement a properly formatted warning message

			State<T> nextState;

			if (table.CanTransit(_currentState, out nextState, data))
				TransitToState(nextState, data);

			_currentState?.OnUpdate(data);
		}

		private void TransitToState(State<T> nextState, T data)
		{
			_currentState?.OnExit(data);
			_currentState = nextState;
			_currentState?.OnEnter(data);
		}

		protected abstract T GetData();
		protected abstract TransitionTable<T> GetTransitionTable();
	}
}
