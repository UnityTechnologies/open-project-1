using UnityEngine;

public class Character : MonoBehaviour
{
	private CharacterController characterController;
	[SerializeField]
	private CharacterData characterData;

	private float turnSmoothSpeed; //Used by Mathf.SmoothDampAngle to smoothly rotate the character to their movement direction
	private float currentSlope;
	private Vector3 hitNormal; // ground normal
	public bool shouldSlide; // Should player slide?
	public bool collisionTop;

	private Vector3 workspace;

	private const float ROTATION_TRESHOLD = .02f; // Used to prevent NaN result causing rotation in a non direction
	public Vector3 InputVector { get; private set; }
	public bool JumpInput { get; private set; }
	public bool JumpInputStop { get; private set; }
	public Vector3 CurrentVelocity { get; private set; }

	public CharacterStateMachine StateMachine { get; private set; }

	public CharacterIdleState IdleState { get; private set; }
	public CharacterMoveState MoveState { get; private set; }
	public CharacterJumpState JumpState { get; private set; }
	public CharacterInAirState InAirState { get; private set; }

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();

		StateMachine = new CharacterStateMachine();

		IdleState = new CharacterIdleState(this, StateMachine, characterData);
		MoveState = new CharacterMoveState(this, StateMachine, characterData);
		JumpState = new CharacterJumpState(this, StateMachine, characterData);
		InAirState = new CharacterInAirState(this, StateMachine, characterData);
	}

	private void Start()
	{
		StateMachine.Initialize(IdleState);
	}

	private void Update()
	{
		StateMachine.CurrentState.LogicUpdate();

		UpdateSlide();

		characterController.Move(CurrentVelocity * Time.deltaTime);

		if (new Vector3(CurrentVelocity.x, 0, CurrentVelocity.z).sqrMagnitude >= ROTATION_TRESHOLD)
		{
			float targetRotation = Mathf.Atan2(CurrentVelocity.x, CurrentVelocity.z) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
				transform.eulerAngles.y,
				targetRotation,
				ref turnSmoothSpeed,
				characterData.turnSmoothTime);
		}
	}

	private void FixedUpdate()
	{
		StateMachine.CurrentState.PhysicsUpdate();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		hitNormal = hit.normal;
		bool isMovingUpwards = CurrentVelocity.y > 0f;
		if (isMovingUpwards)
		{
			// Making sure the collision is near the top of the head
			float permittedDistance = characterController.radius / 2f;
			float topPositionY = transform.position.y + characterController.height;
			float distance = Mathf.Abs(hit.point.y - topPositionY);
			if (distance <= permittedDistance)
			{
				collisionTop = true;
			}
		}
	}
	//---- COMMANDS ISSUED BY OTHER SCRIPTS ----
	public void Move(Vector3 movement)
	{
		InputVector = movement;
	}

	public void Jump()
	{
		JumpInput = true;
	}

	public void CancelJump()
	{
		JumpInput = false;
		InAirState.ResetIsJumping();
	}

	private void UpdateSlide()
	{
		// if player has to slide then add sideways speed to make it go down
		if (shouldSlide)
		{
			SetVelocityX(CurrentVelocity.x + (1f - hitNormal.y) * hitNormal.x * (characterData.speed - characterData.slideFriction));
			SetVelocityZ(CurrentVelocity.z + (1f - hitNormal.y) * hitNormal.z * (characterData.speed - characterData.slideFriction));
		}
		// check if the controller is grounded and above slope limit
		// if player is grounded and above slope limit
		// player has to slide
		if (characterController.isGrounded)
		{
			currentSlope = Vector3.Angle(Vector3.up, hitNormal);
			shouldSlide = currentSlope >= characterController.slopeLimit;
		}
	}

	public void SetVelocityZero()
	{
		CurrentVelocity = Vector2.zero;

	}
	public void SetVelocity(Vector3 velocity)
	{
		workspace = velocity;
		CurrentVelocity = workspace;
	}
	public void SetVelocityX(float velocityX)
	{
		workspace.Set(velocityX, CurrentVelocity.y, CurrentVelocity.z);
		CurrentVelocity = workspace;
	}
	public void SetVelocityY(float velocityY)
	{
		workspace.Set(CurrentVelocity.x, velocityY, CurrentVelocity.z);
		CurrentVelocity = workspace;
	}
	public void SetVelocityZ(float velocityZ)
	{
		workspace.Set(CurrentVelocity.x, CurrentVelocity.y, velocityZ);
		CurrentVelocity = workspace;
	}

	public bool CheckIfGrounded()
	{
		return characterController.isGrounded;
	}
}
