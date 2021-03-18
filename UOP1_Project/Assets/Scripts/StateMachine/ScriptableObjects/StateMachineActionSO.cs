namespace UOP1.StateMachine.ScriptableObjects
{
	using UnityEngine;
	using UOP1.StateMachine;
	using System;
	using System.Collections.Generic;

	[CreateAssetMenu(fileName = "StateMachineAction", menuName = "State Machines/Actions/StateMachine")]
	public class StateMachineActionSO : StateActionSO<StateMachineAction>
	{
		[Tooltip("Set the initial state of this StateMachine")]
		public TransitionTableSO _transitionTableSO = default;
	}

	public class StateMachineAction : StateAction
	{
		private new StateMachineActionSO _originSO => (StateMachineActionSO)base.OriginSO; // The SO this StateAction spawned from
		private readonly Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();
		internal State _currentState;

		private StateMachine _parentStateMachine = default;
		private State _initialState = default;

		public override void Awake(StateMachine stateMachine)
		{
			_parentStateMachine = stateMachine;
			_initialState = _originSO._transitionTableSO.GetInitialState(stateMachine);
		}

		public override void OnStateEnter()
		{
			_currentState = _initialState;
			_currentState.OnStateEnter();
		}

		public override void OnUpdate()
		{
			if (_currentState.TryGetTransition(out var transitionState))
				Transition(transitionState);

			_currentState.OnUpdate();
		}

		public override void OnStateExit()
		{
			_currentState.OnStateExit();
		}

		private void Transition(State transitionState)
		{
			_currentState.OnStateExit();
			_currentState = transitionState;
			_currentState.OnStateEnter();
		}
	}
}
