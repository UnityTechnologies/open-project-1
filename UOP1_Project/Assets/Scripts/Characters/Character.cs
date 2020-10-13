using UnityEngine;

public class Character : MonoBehaviour
{
	private CharacterController characterController;

	[Tooltip("Horizontal XZ plane speed multiplier")] public float speed = 8f;
	[Tooltip("Smoothing for rotating the character to their movement direction")] public float turnSmoothTime = 0.2f;
	[Tooltip("General multiplier for gravity (affects jump and freefall)")] public float gravityMultiplier = 5f;
	[Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")] public float initialJumpForce = 10f;
	[Tooltip("How long can the player hold the jump button")] public float jumpInputDuration = .4f;
	[Tooltip("Represents how fast gravityContributionMultiplier will go back to 1f. The higher, the faster")] public float gravityComebackMultiplier = 15f;
	[Tooltip("The maximum speed reached when falling (in units/frame)")] public float maxFallSpeed = 50f;
	[Tooltip("Each frame while jumping, gravity will be multiplied by this amount in an attempt to 'cancel it' (= jump higher)")] public float gravityDivider = .6f;
	[Tooltip("Adjust the friction of the slope")] public float slideFriction = 0.3f;
	[Tooltip("Starting vertical movement when falling from a platform")] public float fallingVerticalMovement = -5f;


	private float gravityContributionMultiplier = 0f; //The factor which determines how much gravity is affecting verticalMovement
	private bool isJumping = false; //If true, a jump is in effect and the player is holding the jump button
	private float jumpBeginTime = -Mathf.Infinity; //Time of the last jump
	private float turnSmoothSpeed; //Used by Mathf.SmoothDampAngle to smoothly rotate the character to their movement direction
	private float verticalMovement = 0f; //Represents how much a player will move vertically in a frame. Affected by gravity * gravityContributionMultiplier
	private float currentSlope;
	private Vector3 hitNormal; // ground normal
	private bool shouldSlide; // Should player slide?
	private Vector3 inputVector; //Initial input horizontal movement (y == 0f)
	private Vector3 movementVector; //Final movement vector

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
			gravityContributionMultiplier += Time.deltaTime * gravityComebackMultiplier;
		}
		//Reduce the influence of the gravity while holding the Jump button
		if (isJumping)
		{
			//The player can only hold the Jump button for so long
			if (Time.time >= jumpBeginTime + jumpInputDuration)
			{
				isJumping = false;
				gravityContributionMultiplier = 1f; //Gravity influence is reset to full effect
			}
			else
			{
				gravityContributionMultiplier *= gravityDivider; //Reduce the gravity effect
			}
		}
		//Calculate the final verticalMovement
		if (!characterController.isGrounded)
		{
			//Less control in mid-air, conserving momentum from previous frame
			movementVector = inputVector * speed;
			//The character is either jumping or in freefall, so gravity will add up
			gravityContributionMultiplier = Mathf.Clamp01(gravityContributionMultiplier);
			verticalMovement += Physics.gravity.y * gravityMultiplier * Time.deltaTime * gravityContributionMultiplier; //Add gravity contribution
																														//Note that even if it's added, the above value is negative due to Physics.gravity.y
																														//Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
			verticalMovement = Mathf.Clamp(verticalMovement, -maxFallSpeed, 100f);
		}
		else
		{
			//Full speed ground movement
			movementVector = inputVector * speed;
			//Resets the verticalMovement while on the ground,
			//so that regardless of whether the player landed from a high fall or not,
			//if they drop off a platform they will always start with the same verticalMovement.
			//-5f is a good value to make it so the player also sticks to uneven terrain/bumps without floating
			if (!isJumping)
			{
				verticalMovement = fallingVerticalMovement;
				gravityContributionMultiplier = 0f;
			}
		}
		UpdateSlide();
		//Apply the result and move the character in space
		movementVector.y = verticalMovement;
		characterController.Move(movementVector * Time.deltaTime);
		//Rotate to the movement direction
		movementVector.y = 0f;
		if (movementVector.sqrMagnitude >= ROTATION_TRESHOLD)
		{
			float targetRotation = Mathf.Atan2(movementVector.x, movementVector.z) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
				transform.eulerAngles.y,
				targetRotation,
				ref turnSmoothSpeed,
				turnSmoothTime);
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		hitNormal = hit.normal;
		bool isMovingUpwards = verticalMovement > 0f;
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
				isJumping = false;
				gravityContributionMultiplier = 1f;
				verticalMovement = 0f;
			}
		}
	}
	//---- COMMANDS ISSUED BY OTHER SCRIPTS ----
	public void Move(Vector3 movement)
	{
		inputVector = movement;
	}

	public void Jump()
	{
		// Disable jumping if player has to slide
		if (characterController.isGrounded && !shouldSlide)
		{
			isJumping = true;
			jumpBeginTime = Time.time;
			verticalMovement = initialJumpForce; //This is the only place where verticalMovement is set to a positive value
			gravityContributionMultiplier = 0f;
		}
	}

	public void CancelJump()
	{
		isJumping = false; //This will stop the reduction to the gravity, which will then quickly pull down the character
	}

	private void UpdateSlide()
	{
		// if player has to slide then add sideways speed to make it go down
		if (shouldSlide)
		{
			movementVector.x += (1f - hitNormal.y) * hitNormal.x * (speed - slideFriction);
			movementVector.z += (1f - hitNormal.y) * hitNormal.z * (speed - slideFriction);
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
}
