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
	private Damageable _damageableEntity;

	public override void Awake(StateMachine stateMachine)
	{
		_damageableEntity = stateMachine.GetComponent<Damageable>();
	}

	protected override bool Statement()
	{
		bool result = false;
		if (_damageableEntity != null)
		{
			result = _damageableEntity.isDead;
		}
		return result;
	}
}
