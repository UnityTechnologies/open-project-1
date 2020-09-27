using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	private CharacterController characterController;

	[Tooltip("Horizontal XZ plane speed multiplier")]
	public float speed = 8f;

	[Tooltip("General multiplier for gravity (affects jump and freefall)")]
	public float gravityMultiplier = 5f;

	[Tooltip(
		"The initial upwards push when pressing jump. " +
		"This is injected into verticalMovement, and gradually cancelled by gravity")]
	public float initialJumpForce = 10f;

	[Tooltip("How long can the player hold the jump button")]
	public float jumpInputDuration = .4f;

	[Tooltip(
		"Represents how fast gravityContributionMultiplier will go back to 1f. " +
		"The higher, the faster")]
	public float gravityComebackMultiplier = 15f;

	[Tooltip("The maximum speed reached when falling (in units/frame)")]
	public float maxFallSpeed = 50f;

	[Tooltip(
		"Each frame while jumping, gravity will be multiplied " +
		"by this amount in an attempt to 'cancel it' (= jump higher)")]
	public float gravityDivider = .6f;

	// The factor which determines how much gravity is affecting verticalMovement
	private float gravityContributionMultiplier = 0f;
	// If true, a jump is in effect and the player is holding the jump button
	private bool isJumping = false;
	// Time of the last jump
	private float jumpBeginTime = -Mathf.Infinity;
	// Represents how much a player will move vertically in a frame. Affected by gravity * gravityContributionMultiplier
	private float verticalMovement = 0f;
	// Initial input horizontal movement (y == 0f)
	private Vector3 inputVector;
	// Final movement vector
	private Vector3 movementVector;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		// Raises the multiplier to how much gravity will affect vertical movement when in mid-air
		// This is 0f at the beginning of a jump and will raise to maximum 1f
		if (!characterController.isGrounded)
		{
			gravityContributionMultiplier += Time.deltaTime * gravityComebackMultiplier;
		}

		// Reduce the influence of the gravity while holding the Jump button
		if (isJumping)
		{
			// The player can only hold the Jump button for so long
			if (Time.time >= jumpBeginTime + jumpInputDuration)
			{
				isJumping = false;

				// Gravity influence is reset to full effect
				gravityContributionMultiplier = 1f;
			}
			else
			{
				// Reduce the gravity effect
				gravityContributionMultiplier *= gravityDivider; 
			}
		}

		// Calculate the final verticalMovement
		if (!characterController.isGrounded)
		{
			// Less control in mid-air, conserving momentum from previous frame
			movementVector = inputVector * speed;

			// The character is either jumping or in freefall, so gravity will add up
			gravityContributionMultiplier = Mathf.Clamp01(gravityContributionMultiplier);
			// Add gravity contribution
			verticalMovement += Physics.gravity.y * gravityMultiplier * Time.deltaTime * gravityContributionMultiplier;
			// Note that even if it's added, the above value is negative due to Physics.gravity.y

			// Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
			verticalMovement = Mathf.Clamp(verticalMovement, -maxFallSpeed, 100f);
		}
		else
		{
			// Full speed ground movement
			movementVector = inputVector * speed;
			
			// Resets the verticalMovement while on the ground,
			// so that regardless of whether the player landed from a high fall or not,
			// if they drop off a platform they will always start with the same verticalMovement.
			// -5f is a good value to make it so the player also sticks to uneven terrain/bumps without floating.
			if (!isJumping)
			{
				verticalMovement = -5f;
				gravityContributionMultiplier = 0f;
			}
		}

		// Apply the result and move the character in space
		movementVector.y = verticalMovement;
		characterController.Move(movementVector * Time.deltaTime);

		// Rotate to the movement direction
		movementVector.y = 0f;
		if (movementVector.sqrMagnitude >= .02f)
		{
			transform.forward = movementVector.normalized;
		}
	}

	// ---- COMMANDS ISSUED BY OTHER SCRIPTS ----

	public void Move(Vector3 movement)
	{
		inputVector = movement;
	}

	public void Jump()
	{
		if (characterController.isGrounded)
		{
			isJumping = true;
			jumpBeginTime = Time.time;

			// This is the only place where verticalMovement is set to a positive value
			verticalMovement = initialJumpForce;

			gravityContributionMultiplier = 0f;
		}
	}

	public void CancelJump()
	{
		// This will stop the reduction to the gravity, which will then quickly pull down the character
		isJumping = false;
	}
}
