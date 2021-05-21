using UnityEngine;
using UnityEngine.Localization;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsNPCSayingTheLine", menuName = "State Machines/Conditions/Is NPC Saying The Line")]
public class IsNPCSayingTheLineSO : StateConditionSO {

	[SerializeField] private DialogueLineChannelSO _onLineDisplayed = default;
	[SerializeField] private ActorSO _protagonistActor;

	protected override Condition CreateCondition() => new IsNPCSayingTheLineCondition(_onLineDisplayed, _protagonistActor);

}

public class IsNPCSayingTheLineCondition : Condition
{
	private DialogueLineChannelSO _sayLineEvent;
	private ActorSO _protagonistActor;
	private bool _isNPCSayingTheLine = false;

	public IsNPCSayingTheLineCondition(DialogueLineChannelSO sayLineEvent, ActorSO protagonistActor)
	{
		_sayLineEvent = sayLineEvent;
		_protagonistActor = protagonistActor;
	}
	protected override bool Statement()
	{

		return _isNPCSayingTheLine;
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
	}

	private void OnLineDisplayed(LocalizedString line, ActorSO actor)
	{
		if (actor.ActorName == _protagonistActor.ActorName)
		{
			_isNPCSayingTheLine = false;
		}
		else
		{
			_isNPCSayingTheLine = true;
		}
	}
}
