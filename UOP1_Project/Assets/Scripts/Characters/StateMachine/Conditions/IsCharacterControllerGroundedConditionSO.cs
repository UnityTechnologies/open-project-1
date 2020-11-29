using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Character Controller Grounded")]
public class IsCharacterControllerGroundedConditionSO : StateConditionSO<IsCharacterControllerGroundedCondition> { }

public class IsCharacterControllerGroundedCondition : Condition
{
	private CharacterController _characterController;

	public override void Awake(StateMachine stateMachine)
	{
		_characterController = stateMachine.GetComponent<CharacterController>();
	}

	protected override bool Statement() => _characterController.isGrounded;
}
