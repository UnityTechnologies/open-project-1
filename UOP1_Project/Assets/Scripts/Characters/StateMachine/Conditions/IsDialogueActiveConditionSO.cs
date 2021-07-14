using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsDialogueActiveCondition", menuName = "State Machines/Conditions/Is Dialogue Active Condition")]
public class IsDialogueActiveConditionSO : StateConditionSO
{
	[SerializeField] private DialogueDataChannelSO _startDialogueEvent = default;
	[SerializeField] private IntEventChannelSO _endDialogueEvent = default;

	protected override Condition CreateCondition() => new IsDialogueActiveCondition(_startDialogueEvent, _endDialogueEvent);

}

public class IsDialogueActiveCondition : Condition
{
	private DialogueDataChannelSO _startDialogueEvent;
	private IntEventChannelSO _endDialogueEvent;
	private bool _isDialogueActive = false;

	public IsDialogueActiveCondition(DialogueDataChannelSO startDialogueEvent, IntEventChannelSO endDialogueEvent)
	{
		_startDialogueEvent = startDialogueEvent;
		_endDialogueEvent = endDialogueEvent;
	}

	protected override bool Statement()
	{
		return _isDialogueActive;
	}

	public override void OnStateEnter()
	{
		if (_startDialogueEvent != null)
		{
			_startDialogueEvent.OnEventRaised += OnDialogueStart;
		}

		if (_endDialogueEvent != null)
		{
			_endDialogueEvent.OnEventRaised += OnDialogueEnd;
		}
	}

	public override void OnStateExit()
	{
		if (_startDialogueEvent != null)
		{
			_startDialogueEvent.OnEventRaised -= OnDialogueStart;
		}

		if (_endDialogueEvent != null)
		{
			_endDialogueEvent.OnEventRaised -= OnDialogueEnd;
		}
	}

	private void OnDialogueStart(DialogueDataSO dialogue)
	{
		_isDialogueActive = true;
	}

	private void OnDialogueEnd(int dialogueType)
	{
		_isDialogueActive = false;
	}
}
