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
			result = _attackableEntity.getHit;
		}
		return result;
	}
}
