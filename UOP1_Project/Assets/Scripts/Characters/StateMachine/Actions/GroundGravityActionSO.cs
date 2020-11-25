using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GroundGravity", menuName = "State Machines/Actions/Ground Gravity")]
public class GroundGravityActionSO : StateActionSO
{
	[Tooltip("Vertical movement pulling down the player to keep it anchored to the ground.")]
	[SerializeField] private float _verticalPull = -5f;

	protected override StateAction CreateAction() => new GroundGravityAction(_verticalPull);
}

public class GroundGravityAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;

	private float _verticalPull;

	public GroundGravityAction(float slideSpeed)
	{
		_verticalPull = slideSpeed;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		_protagonistScript.movementVector.y = _verticalPull;
	}
}
