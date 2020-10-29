using System;
using System.Text;
using UnityEngine;

namespace UOP1.StateMachine
{
	[Serializable]
	internal class StateMachineDebugger
	{
		[SerializeField]
		internal bool debugTransitions = false;

		[SerializeField]
		internal string CurrentState;

		private StateMachine _stateMachine;
		private StringBuilder _logBuilder;
		private string _targetState = string.Empty;

		internal void Awake(StateMachine stateMachine, string initialState)
		{
			_stateMachine = stateMachine;
			_logBuilder = new StringBuilder();

			CurrentState = initialState;
		}

		internal void TransitionEvaluationBegin(string targetState)
		{
			_targetState = targetState;

			if (!debugTransitions)
				return;

			_logBuilder.Clear();
			_logBuilder.AppendLine($"{_stateMachine.gameObject.name} state changed");
			_logBuilder.Append($"{CurrentState} > > > {_targetState}");
		}

		internal void TransitionConditionResult(Type conditionType, bool result, bool isMet)
		{
			if (!debugTransitions || _logBuilder.Length == 0)
				return;

			_logBuilder.AppendLine();
			_logBuilder.Append($"> {conditionType} == {result}");

			if (isMet) //Unicode checked and Unchecked marks
				_logBuilder.Append(" [\u2714]");
			else
				_logBuilder.Append(" [\u2718]");
		}

		internal void TransitionEvaluationEnd(bool passed)
		{
			if (passed)
				CurrentState = _targetState;

			if (!debugTransitions || _logBuilder.Length == 0)
				return;

			if (passed)
				PrintDebugLog();

			_logBuilder.Clear();
		}

		private void PrintDebugLog()
		{
			_logBuilder.AppendLine();
			_logBuilder.Append("--------------------------------");

			Debug.Log(_logBuilder.ToString());
		}
	}
}
