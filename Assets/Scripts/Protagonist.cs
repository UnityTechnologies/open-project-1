using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : MonoBehaviour
{
	public InputReader inputReader;
	private CharacterController characterController;

	public float speed = 10f;
	public float gravityMultiplier = 5f;
	public float initialJumpForce = 10f;
	public float jumpInputDuration = .4f;

	public float maxFallSpeed = 50f;

	[SerializeField] private float gravityContributionMultiplier = 0f;
	[SerializeField] private float gravityDivider = .7f;
	[SerializeField] private float gravityComebackMultiplier = 5f;
	private bool isJumping = false;
	private float jumpBeginTime = -Mathf.Infinity;
	private float verticalMovement = 0f;
	private Vector3 movementVector;
	private Vector3 inputVector;

	//Adds listeners for events being triggered in the InputReader script
	private void OnEnable()
	{
		inputReader.jumpEvent += OnJumpInitiated;
		inputReader.jumpCanceledEvent += OnJumpCanceled;
		inputReader.moveEvent += OnMove;
		//...
	}

	//Removes all listeners to the events coming from the InputReader script
	private void OnDisable()
	{
		//...
	}

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		//Raises the multiplier to how much gravity will affect vertical movement when in mid-air
		//This is 0f at the beginning of a jump and will raise to maximum 1f
		if(!characterController.isGrounded)
		{
			gravityContributionMultiplier += Time.deltaTime * gravityComebackMultiplier;
		}

		//Reduce the influence of the gravity while holding the Jump button
		if(isJumping)
		{
			//The player can only hold the Jump button for so long
			if(Time.time >= jumpBeginTime + jumpInputDuration)
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
		if(!characterController.isGrounded)
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
			//-5f is a good value to make it so the player also sticks to uneven terrain/bumps without floating.
			if(!isJumping)
			{
				verticalMovement = -5f;
				gravityContributionMultiplier = 0f;
			}
		}

		//Apply the result and move the character in space
		movementVector.y = verticalMovement;
		characterController.Move(movementVector * Time.deltaTime);

		//Rotate to the movement direction
		movementVector.y = 0f;
		if(movementVector.sqrMagnitude >= .02f)
		{
			transform.forward = movementVector.normalized;
		}
	}

	private void OnMove(Vector2 movement)
	{
		inputVector = new Vector3(movement.x, 0f, movement.y);
	}

	private void OnJumpInitiated()
	{
		if(characterController.isGrounded)
		{
			isJumping = true;
			jumpBeginTime = Time.time;
			verticalMovement = initialJumpForce; //This is the only place where verticalMovement is set to a positive value
			gravityContributionMultiplier = 0f;
		}
	}

	private void OnJumpCanceled()
	{
		isJumping = false; //This will stop the reduction to the gravity, which will then quickly pull down the character
	}
}