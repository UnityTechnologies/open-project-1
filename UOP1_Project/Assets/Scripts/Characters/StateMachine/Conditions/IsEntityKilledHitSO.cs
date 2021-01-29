using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsEntityKilledHit", menuName = "State Machines/Conditions/Is Entity Killed")]
public class IsEntityKilledHitSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsEntityKilledHit();
}

public class IsEntityKilledHit : Condition
{
	private AttackableEntity _attackableEntity;

	public override void Awake(StateMachine stateMachine)
	{
		_attackableEntity = stateMachine.GetComponent<AttackableEntity>();
	}

	protected override bool Statement()
	{
		bool result = false;
		if (_attackableEntity != null)
		{
			result = _attackableEntity.isDead;
		}
		return result;
	}
}
