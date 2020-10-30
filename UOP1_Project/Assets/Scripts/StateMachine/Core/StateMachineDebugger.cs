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
		internal bool appendConditionsInfo = true;

		[SerializeField]
		internal bool appendActionsInfo = true;

		[SerializeField]
		internal string currentState;

		private StateMachine _stateMachine;
		private StringBuilder _logBuilder;
		private string _targetState = string.Empty;

		internal void Awake(StateMachine stateMachine, string initialState)
		{
			_stateMachine = stateMachine;
			_logBuilder = new StringBuilder();

			currentState = initialState;
		}

		internal void TransitionEvaluationBegin(string transitionName, string targetState)
		{
			_targetState = targetState;

			if (!debugTransitions)
				return;

			_logBuilder.Clear();
			_logBuilder.AppendLine($"{_stateMachine.gameObject.name} state changed");
			_logBuilder.AppendLine($"{currentState}  >>>  {_targetState}");

			if (appendConditionsInfo)
			{
				_logBuilder.AppendLine();
				_logBuilder.AppendLine($"Transition: {transitionName}");
			}
		}

		internal void TransitionConditionResult(string conditionName, bool result, bool isMet)
		{
			if (!debugTransitions || _logBuilder.Length == 0 || !appendConditionsInfo)
				return;

			_logBuilder.Append($"    \u279C {conditionName} == {result}");

			if (isMet) //Unicode checked and Unchecked marks
				_logBuilder.AppendLine(" [\u2714]");
			else
				_logBuilder.AppendLine(" [\u2718]");
		}

		internal void TransitionEvaluationEnd(bool passed, StateAction[] actions)
		{
			if (passed)
				currentState = _targetState;

			if (!debugTransitions || _logBuilder.Length == 0)
				return;

			if (passed)
			{
				LogActions(actions);
				PrintDebugLog();
			}

			_logBuilder.Clear();
		}

		private void LogActions(StateAction[] actions)
		{
			if (!appendActionsInfo)
				return;

			_logBuilder.AppendLine();
			_logBuilder.AppendLine("State Actions:");

			foreach (StateAction action in actions)
			{
				_logBuilder.AppendLine($"    \u279C {action._originSO.name}");
			}
		}

		private void PrintDebugLog()
		{
			_logBuilder.AppendLine();
			_logBuilder.Append("--------------------------------");

			Debug.Log(_logBuilder.ToString());
		}
	}
}
