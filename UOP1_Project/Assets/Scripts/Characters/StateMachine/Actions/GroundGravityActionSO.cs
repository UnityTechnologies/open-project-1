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
	private Character _characterScript;

	private float _verticalPull;

	public GroundGravityAction(float slideSpeed)
	{
		_verticalPull = slideSpeed;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_characterScript = stateMachine.GetComponent<Character>();
	}

	public override void OnUpdate()
	{
		_characterScript.movementVector.y = _verticalPull;
	}
}
