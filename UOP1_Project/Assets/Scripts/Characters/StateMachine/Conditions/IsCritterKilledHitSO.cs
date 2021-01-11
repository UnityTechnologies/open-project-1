using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsCritterKilled", menuName = "State Machines/Conditions/Is Critter Killed")]
public class IsCritterKilledHitSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsCritterKilled();
}

public class IsCritterKilled : Condition
{
	private Critter _critter;

	public override void Awake(StateMachine stateMachine)
	{
		_critter = stateMachine.GetComponent<Critter>();
	}

	protected override bool Statement()
	{
		bool result = false;
		if (_critter != null)
		{
			result = _critter.isDead;
		}
		return result;
	}
}
