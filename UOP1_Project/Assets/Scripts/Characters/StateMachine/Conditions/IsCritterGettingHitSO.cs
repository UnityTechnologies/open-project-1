using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "IsCritterGettingHit", menuName = "State Machines/Conditions/Is Critter Getting Hit")]
public class IsCritterGettingHitSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsCritterGettingHit();
}

public class IsCritterGettingHit : Condition
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
			result = _critter.getHit;
		}
		return result;
	}
}
