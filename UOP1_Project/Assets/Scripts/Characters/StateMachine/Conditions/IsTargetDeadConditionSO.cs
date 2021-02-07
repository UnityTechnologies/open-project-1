using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsTargetDeadCondition", menuName = "State Machines/Conditions/Is Target Dead Condition")]
public class IsTargetDeadConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsTargetDeadCondition();
}

public class IsTargetDeadCondition : Condition
{
	private Critter _critterScript;

	public override void Awake(StateMachine stateMachine)
	{
		_critterScript = stateMachine.GetComponent<Critter>();
	}

	protected override bool Statement()
	{
		return _critterScript.currentTarget == null || _critterScript.currentTarget.IsDead;
	}
}
