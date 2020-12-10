using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Holding Jump")]
public class IsHoldingJumpConditionSO : StateConditionSO<IsHoldingJumpCondition> { }

public class IsHoldingJumpCondition : Condition
{
	//Component references
	private Protagonist _protagonistScript;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	protected override bool Statement() => _protagonistScript.jumpInput;
}
