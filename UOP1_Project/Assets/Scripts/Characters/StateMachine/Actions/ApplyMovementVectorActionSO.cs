using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ApplyMovementVector", menuName = "State Machines/Actions/Apply Movement Vector")]
public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction> { }

public class ApplyMovementVectorAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;
	private CharacterController _characterController;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
		_characterController = stateMachine.GetComponent<CharacterController>();
	}

	public override void OnUpdate()
	{
		_characterController.Move(_protagonistScript.movementVector * Time.deltaTime);
		_protagonistScript.movementVector = _characterController.velocity;
	}
}
