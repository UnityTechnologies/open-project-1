using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsHoldingJump", menuName = "State Machines/Conditions/Is Holding Jump")]
public class IsHoldingJumpConditionSO : StateConditionSO<IsHoldingJumpCondition> { }

public class IsHoldingJumpCondition : Condition
{
	//Component references
	private Protagonist _characterScript;

	public override void Awake(StateMachine stateMachine)
	{
		_characterScript = stateMachine.GetComponent<Protagonist>();
	}

	protected override bool Statement() => _characterScript.jumpInput;
}
