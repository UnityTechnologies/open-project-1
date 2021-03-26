using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Holding Attack Action")]
public class IsHoldingAttackActionConditionSO : StateConditionSO<IsHoldingAttackActionCondition> { }

public class IsHoldingAttackActionCondition : Condition
{
	//Component references
	private Protagonist _protagonistScript;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	protected override bool Statement()
	{
		return _protagonistScript.attackInput;
	}
}
