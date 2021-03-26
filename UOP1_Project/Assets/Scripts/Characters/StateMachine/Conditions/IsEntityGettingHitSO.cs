using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "IsEntityGettingHit", menuName = "State Machines/Conditions/Is Entity Getting Hit")]
public class IsEntityGettingHitSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsEntityGettingHit();
}

public class IsEntityGettingHit : Condition
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
			result = _damageableEntity.GetHit;
		}
		return result;
	}
}
