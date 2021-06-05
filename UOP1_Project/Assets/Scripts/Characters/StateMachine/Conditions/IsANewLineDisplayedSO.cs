using UnityEngine;
using UnityEngine.Localization;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsANewLineDisplayed", menuName = "State Machines/Conditions/Is A New Line Displayed")]
public class IsANewLineDisplayedSO : StateConditionSO {

	[SerializeField] private DialogueLineChannelSO _onLineDisplayed = default;

	protected override Condition CreateCondition() => new IsANewLineDisplayedCondition(_onLineDisplayed);

}

public class IsANewLineDisplayedCondition : Condition
{
	private DialogueLineChannelSO _sayLineEvent;
	private bool _isAnewLineDisplayed = false;


	public IsANewLineDisplayedCondition(DialogueLineChannelSO sayLineEvent)
	{
		_sayLineEvent = sayLineEvent;
	}
	protected override bool Statement()
	{

		return _isAnewLineDisplayed;
	}

	public override void OnStateEnter()
	{
		if (_sayLineEvent != null)
		{
			_sayLineEvent.OnEventRaised += OnLineDisplayed;
		}
	}

	public override void OnStateExit()
	{
		if (_sayLineEvent != null)
		{
			_sayLineEvent.OnEventRaised -= OnLineDisplayed;
		}
		_isAnewLineDisplayed = false;
	}

	private void OnLineDisplayed(LocalizedString line, ActorSO actor)
	{
		_isAnewLineDisplayed = true;
	}
}
