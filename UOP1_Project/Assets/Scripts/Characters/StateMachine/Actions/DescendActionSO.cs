using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Actions/Descend")]
public class DescendActionSO : StateActionSO<DescendAction>
{
	[Tooltip("Minimum fall time before checking if the player is falling out of bounds")]
	public float minFallTime = 5f;
	
	public TransformEventChannelSO playerTeleportedChannel = default;
}

public class DescendAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;
	private CharacterController _characterController;

	private float _verticalMovement;
	private float _fallTimer;

	private DescendActionSO _originSO => (DescendActionSO)base.OriginSO;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
		_characterController = stateMachine.GetComponent<CharacterController>();
	}

	public override void OnStateEnter()
	{
		_verticalMovement = _protagonistScript.movementVector.y;

		//Prevents a double jump if the player keeps holding the jump button
		//Basically it "consumes" the input
		_protagonistScript.jumpInput = false;

		_fallTimer = _originSO.minFallTime;
	}

	public override void OnUpdate()
	{
		_verticalMovement += Physics.gravity.y * Protagonist.GRAVITY_MULTIPLIER * Time.deltaTime;
		//Note that even if it's added, the above value is negative due to Physics.gravity.y

		//Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
		_verticalMovement = Mathf.Clamp(_verticalMovement, Protagonist.MAX_FALL_SPEED, Protagonist.MAX_RISE_SPEED);

		_protagonistScript.movementVector.y = _verticalMovement;

		_fallTimer -= Time.deltaTime;
		
		if (_fallTimer <= 0f)
		{
			if (!HasGroundBelow())
			{
				TeleportToLastValidPos();
			}
			else
			{
				_fallTimer = _originSO.minFallTime;
			}
		}
	}

	private void TeleportToLastValidPos()
	{
		_characterController.transform.position = _protagonistScript.lastValidPos;
		
		//Force update positions to make sure the character controller does not override movement
		//an alternative to that would be to disable the controller and reenable it again after setting the position
		Physics.SyncTransforms();
		
		//Fire an event that tells the camera to instantly warp to the player
		_originSO.playerTeleportedChannel.RaiseEvent(_characterController.transform);
	}

	private bool HasGroundBelow()
	{
		return Physics.Raycast(_characterController.transform.position, Vector3.down,999f);
	}
}
