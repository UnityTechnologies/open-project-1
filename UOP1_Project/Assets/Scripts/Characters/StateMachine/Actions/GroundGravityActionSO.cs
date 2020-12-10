using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GroundGravity", menuName = "State Machines/Actions/Ground Gravity")]
public class GroundGravityActionSO : StateActionSO<GroundGravityAction>
{
	[Tooltip("Vertical movement pulling down the player to keep it anchored to the ground.")]
	public float verticalPull = -5f;
}

public class GroundGravityAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;

	private GroundGravityActionSO _originSO => (GroundGravityActionSO)base.OriginSO; // The SO this StateAction spawned from

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		_protagonistScript.movementVector.y = _originSO.verticalPull;
	}
}
