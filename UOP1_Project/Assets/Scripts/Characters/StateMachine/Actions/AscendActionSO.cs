using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Ascend", menuName = "State Machines/Actions/Ascend")]
public class AscendActionSO : StateActionSO<AscendAction>
{
	[Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
	public float initialJumpForce = 6f;
}

public class AscendAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;

	private float _verticalMovement;
	private float _gravityContributionMultiplier;
	private AscendActionSO _originSO => (AscendActionSO)base.OriginSO; // The SO this StateAction spawned from

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnStateEnter()
	{
		_verticalMovement = _originSO.initialJumpForce;
	}

	public override void OnUpdate()
	{
		_gravityContributionMultiplier += Protagonist.GRAVITY_COMEBACK_MULTIPLIER;
		_gravityContributionMultiplier *= Protagonist.GRAVITY_DIVIDER; //Reduce the gravity effect
		_verticalMovement += Physics.gravity.y * Protagonist.GRAVITY_MULTIPLIER * Time.deltaTime * _gravityContributionMultiplier;
		//Note that even if it's added, the above value is negative due to Physics.gravity.y

		_protagonistScript.movementVector.y = _verticalMovement;
	}
}
