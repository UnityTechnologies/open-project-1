using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsCharacterReadyToAttack", menuName = "State Machines/Conditions/Is Character Ready To Attack")]
public class IsCharacterReadyToAttackSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsCharacterReadyToAttack();
}

public class IsCharacterReadyToAttack : Condition
{
	private IAttackerCharacter _attacker;

	public override void Awake(StateMachine stateMachine)
	{
		_attacker = stateMachine.GetComponents<IAttackerCharacter>()[0];
	}


	protected override bool Statement()
	{
		bool result = false;
		if (_attacker != null)
		{
			result = _attacker.IsReadyToAttack();
		}
		return result;
	}
}
