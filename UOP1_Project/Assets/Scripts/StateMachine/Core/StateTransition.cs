﻿using UnityEngine;

namespace UOP1.StateMachine
{
	public class StateTransition : IStateComponent
	{
		private readonly ScriptableObject _originSO;
		private readonly State _targetState;
		private readonly StateCondition[] _conditions;
		private readonly int[] _resultGroups;
		private readonly bool[] _results;
		

		public StateTransition(ScriptableObject originSO, State targetState, StateCondition[] conditions, int[] resultGroups = null)
		{
			_originSO = originSO;
			_targetState = targetState;
			_conditions = conditions;
			_resultGroups = resultGroups != null && resultGroups.Length > 0 ? resultGroups : new int[1];
			_results = new bool[_resultGroups.Length];
		}

		/// <summary>
		/// Checks wether the conditions to transition to the target state are met.
		/// </summary>
		/// <param name="state">Returns the state to transition to. Null if the conditions aren't met.</param>
		/// <returns>True if the conditions are met.</returns>
		public bool TryGetTransiton(out State state)
		{
			state = ShouldTransition() ? _targetState : null;
			return state != null;
		}

		public void OnStateEnter()
		{
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i]._condition.OnStateEnter();
		}

		public void OnStateExit()
		{
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i]._condition.OnStateExit();
		}

		private bool ShouldTransition()
		{
#if UNITY_EDITOR
			_targetState._stateMachine._debugger.TransitionEvaluationBegin(_originSO.name, _targetState.Name);
#endif

			int count = _resultGroups.Length;
			for (int i = 0, idx = 0; i < count && idx < _conditions.Length; i++)
				for (int j = 0; j < _resultGroups[i]; j++, idx++)
					_results[i] = j == 0 ?
						_conditions[idx].IsMet() :
						_results[i] && _conditions[idx].IsMet();

			bool ret = false;
			for (int i = 0; i < count && !ret; i++)
				ret = ret || _results[i];

#if UNITY_EDITOR
			_targetState._stateMachine._debugger.TransitionEvaluationEnd(ret);
#endif

			return ret;
		}
	}
}
