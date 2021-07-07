using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

/// <summary>
/// Adds the dash movement vector to the protagonist's movementVector
/// </summary>
[CreateAssetMenu(fileName = "AddDashMovement", menuName = "State Machines/Actions/Add Dash Movement")]
public class AddDashMovementActionSO : StateActionSO<AddDashMovementAction>
{
}

public class AddDashMovementAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;
	private Dasher _dasher;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
		_dasher = stateMachine.GetComponent<Dasher>();
	}

	public override void OnUpdate()
	{
		_protagonistScript.movementVector += _dasher.DashMovementVector;
	}
}
