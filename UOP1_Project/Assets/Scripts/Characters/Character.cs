using UnityEngine;

public class Character : MonoBehaviour
{
	private CharacterController characterController;

	[Tooltip("Horizontal XZ plane speed multiplier")] [SerializeField] private float _speed = 8f;
	[Tooltip("Smoothing for rotating the character to their movement direction")] [SerializeField] private float _turnSmoothTime = 0.2f;
	[Tooltip("General multiplier for gravity (affects jump and freefall)")] [SerializeField] private float _gravityMultiplier = 5f;
	[Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")] [SerializeField] private float _initialJumpForce = 10f;
	[Tooltip("How long can the player hold the jump button")] [SerializeField] private float _jumpInputDuration = .4f;
	[Tooltip("Represents how fast gravityContributionMultiplier will go back to 1f. The higher, the faster")] [SerializeField] private float gravityComebackMultiplier = 15f;
	[Tooltip("The maximum speed reached when falling (in units/frame)")] [SerializeField] private float _maxFallSpeed = 50f;
	[Tooltip("Each frame while jumping, gravity will be multiplied by this amount in an attempt to 'cancel it' (= jump higher)")] [SerializeField] private float _gravityDivider = .6f;
	[Tooltip("Adjust the friction of the slope")] [SerializeField] private float _slideFriction = 0.3f;
	[Tooltip("Starting vertical movement when falling from a platform")] [SerializeField] private float _fallingVerticalMovement = -5f;

	private float _gravityContributionMultiplier = 0f; //The factor which determines how much gravity is affecting verticalMovement
	private bool _isJumping = false; //If true, a jump is in effect and the player is holding the jump button
	private float _jumpBeginTime = -Mathf.Infinity; //Time of the last jump
	private float _turnSmoothSpeed; //Used by Mathf.SmoothDampAngle to smoothly rotate the character to their movement direction
	private float _verticalMovement = 0f; //Represents how much a player will move vertically in a frame. Affected by gravity * gravityContributionMultiplier
	private float _currentSlope;
	private Vector3 _hitNormal; // ground normal
	private bool _shouldSlide; // Should player slide?
	private Vector3 _inputVector; //Initial input horizontal movement (y == 0f)
	private Vector3 _movementVector; //Final movement vector

	private const float ROTATION_TRESHOLD = .02f; // Used to prevent NaN result causing rotation in a non direction

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		//Raises the multiplier to how much gravity will affect vertical movement when in mid-air
		//This is 0f at the beginning of a jump and will raise to maximum 1f
		if (!characterController.isGrounded)
		{
			_gravityContributionMultiplier += Time.deltaTime * gravityComebackMultiplier;
		}
		//Reduce the influence of the gravity while holding the Jump button
		if (_isJumping)
		{
			//The player can only hold the Jump button for so long
			if (Time.time >= _jumpBeginTime + _jumpInputDuration)
			{
				_isJumping = false;
				_gravityContributionMultiplier = 1f; //Gravity influence is reset to full effect
			}
			else
			{
				_gravityContributionMultiplier *= _gravityDivider; //Reduce the gravity effect
			}
		}
		//Calculate the final verticalMovement
		if (!characterController.isGrounded)
		{
			//Less control in mid-air, conserving momentum from previous frame
			_movementVector = _inputVector * _speed;
			//The character is either jumping or in freefall, so gravity will add up
			_gravityContributionMultiplier = Mathf.Clamp01(_gravityContributionMultiplier);
			_verticalMovement += Physics.gravity.y * _gravityMultiplier * Time.deltaTime * _gravityContributionMultiplier; //Add gravity contribution
																														//Note that even if it's added, the above value is negative due to Physics.gravity.y
																														//Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
			_verticalMovement = Mathf.Clamp(_verticalMovement, -_maxFallSpeed, 100f);
		}
		else
		{
			//Full speed ground movement
			_movementVector = _inputVector * _speed;
			//Resets the verticalMovement while on the ground,
			//so that regardless of whether the player landed from a high fall or not,
			//if they drop off a platform they will always start with the same verticalMovement.
			//-5f is a good value to make it so the player also sticks to uneven terrain/bumps without floating
			if (!_isJumping)
			{
				_verticalMovement = _fallingVerticalMovement;
				_gravityContributionMultiplier = 0f;
			}
		}
		UpdateSlide();
		//Apply the result and move the character in space
		_movementVector.y = _verticalMovement;
		characterController.Move(_movementVector * Time.deltaTime);
		//Rotate to the movement direction
		_movementVector.y = 0f;
		if (_movementVector.sqrMagnitude >= ROTATION_TRESHOLD)
		{
			float targetRotation = Mathf.Atan2(_movementVector.x, _movementVector.z) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
				transform.eulerAngles.y,
				targetRotation,
				ref _turnSmoothSpeed,
				_turnSmoothTime);
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		_hitNormal = hit.normal;
		bool isMovingUpwards = _verticalMovement > 0f;
		if (isMovingUpwards)
		{
			// Making sure the collision is near the top of the head
			float permittedDistance = characterController.radius / 2f;
			float topPositionY = transform.position.y + characterController.height;
			float distance = Mathf.Abs(hit.point.y - topPositionY);
			if (distance <= permittedDistance)
			{
				// Stopping any upwards movement
				// and having the player fall back down
				_isJumping = false;
				_gravityContributionMultiplier = 1f;
				_verticalMovement = 0f;
			}
		}
	}
	//---- COMMANDS ISSUED BY OTHER SCRIPTS ----
	public void Move(Vector3 movement)
	{
		_inputVector = movement;
	}

	public void Jump()
	{
		// Disable jumping if player has to slide
		if (characterController.isGrounded && !_shouldSlide)
		{
			_isJumping = true;
			_jumpBeginTime = Time.time;
			_verticalMovement = _initialJumpForce; //This is the only place where verticalMovement is set to a positive value
			_gravityContributionMultiplier = 0f;
		}
	}

	public void CancelJump()
	{
		_isJumping = false; //This will stop the reduction to the gravity, which will then quickly pull down the character
	}

	private void UpdateSlide()
	{
		// if player has to slide then add sideways speed to make it go down
		if (_shouldSlide)
		{
			_movementVector.x += (1f - _hitNormal.y) * _hitNormal.x * (_speed - _slideFriction);
			_movementVector.z += (1f - _hitNormal.y) * _hitNormal.z * (_speed - _slideFriction);
		}
		// check if the controller is grounded and above slope limit
		// if player is grounded and above slope limit
		// player has to slide
		if (characterController.isGrounded)
		{
			_currentSlope = Vector3.Angle(Vector3.up, _hitNormal);
			_shouldSlide = _currentSlope >= characterController.slopeLimit;
		}
	}
}
