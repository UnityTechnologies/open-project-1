using UnityEngine;
using UnityEngine.Localization;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsNPCSayingTheLine", menuName = "State Machines/Conditions/Is NPC Saying The Line")]
public class IsNPCSayingTheLineSO : StateConditionSO {

	[SerializeField] private DialogueLineChannelSO _sayLineEvent = default;
	[SerializeField] private ActorSO _protagonistActor;

	protected override Condition CreateCondition() => new IsNPCSayingTheLineCondition(_sayLineEvent, _protagonistActor);

}

public class IsNPCSayingTheLineCondition : Condition
{
	private DialogueLineChannelSO _sayLineEvent;
	private ActorSO _protagonistActor;
	private bool _isNPCSayingTheLine = false;
	private NPC _npcScript;

	public IsNPCSayingTheLineCondition(DialogueLineChannelSO sayLineEvent, ActorSO protagonistActor)
	{
		_sayLineEvent = sayLineEvent;
		_protagonistActor = protagonistActor;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_npcScript = stateMachine.gameObject.GetComponent<NPC>();
	}

	protected override bool Statement()
	{

		return _isNPCSayingTheLine;
	}

	public override void OnStateEnter()
	{
		if (_sayLineEvent != null)
		{
			_sayLineEvent.OnEventRaised += OnLineDisplay;
		}
	}

	public override void OnStateExit()
	{
		if (_sayLineEvent != null)
		{
			_sayLineEvent.OnEventRaised -= OnLineDisplay;
		}
	}

	private void OnLineDisplay(LocalizedString line, ActorSO actor)
	{
		//why is it not possible to do this from here?
		//SetTheLineToNotSaidYet();
		if (actor.ActorName == _protagonistActor.ActorName)
		{
			_isNPCSayingTheLine = true;
		}
		else
		{
			_isNPCSayingTheLine = true;
		}
	}

	private void SetTheLineToNotSaidYet()
	{
		_npcScript.hasSaidLine = false;
	}


}
