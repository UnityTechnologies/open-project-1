using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Ascend", menuName = "State Machines/Actions/Ascend")]
public class AscendActionSO : StateActionSO
{
	[Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
	[SerializeField] private float _initialJumpForce = 10f;

	protected override StateAction CreateAction() => new AscendAction(_initialJumpForce);
}

public class AscendAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;

	private float _verticalMovement;
	private float _gravityContributionMultiplier;
	private float _initialJumpForce;
	private const float GRAVITY_COMEBACK_MULTIPLIER = 15f;
	private const float GRAVITY_DIVIDER = .6f;
	private const float GRAVITY_MULTIPLIER = 5f;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	public AscendAction(float initialJumpForce)
	{
		_initialJumpForce = initialJumpForce;
	}

	public override void OnStateEnter()
	{
		_verticalMovement = _initialJumpForce;
	}

	public override void OnUpdate()
	{
		_gravityContributionMultiplier += Time.deltaTime * GRAVITY_COMEBACK_MULTIPLIER;
		_gravityContributionMultiplier *= GRAVITY_DIVIDER; //Reduce the gravity effect
		_verticalMovement += Physics.gravity.y * GRAVITY_MULTIPLIER * Time.deltaTime * _gravityContributionMultiplier;
		//Note that even if it's added, the above value is negative due to Physics.gravity.y

		_protagonistScript.movementVector.y = _verticalMovement;
	}
}
