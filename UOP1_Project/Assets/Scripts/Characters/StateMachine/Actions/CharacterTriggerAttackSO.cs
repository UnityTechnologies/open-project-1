using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "CharacterTriggerAttack", menuName = "State Machines/Actions/Character Triggers An Attack")]
public class CharacterTriggerAttackSO : StateActionSO
{
	protected override StateAction CreateAction() => new CharacterTriggerAttack();
}

public class CharacterTriggerAttack : StateAction
{
	private IAttackerCharacter _attacker;

	public override void Awake(StateMachine stateMachine)
	{
		_attacker = stateMachine.GetComponents<IAttackerCharacter>()[0];
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		_attacker.TriggerAttack();
	}
}
