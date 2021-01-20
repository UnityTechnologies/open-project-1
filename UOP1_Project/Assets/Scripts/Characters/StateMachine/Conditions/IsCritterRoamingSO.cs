using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "IsCritterRoaming", menuName = "State Machines/Conditions/Is Critter Roaming")]
public class IsCritterRoamingSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsCritterRoaming();
}

public class IsCritterRoaming : Condition
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
			result = _critter.isRoaming;
		}
		return result;
	}
}
