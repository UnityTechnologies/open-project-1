using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : MonoBehaviour
{
	public InputReader inputReader;
	private CharacterController characterController;

	public float speed = 10f;
	public float gravityMultiplier = 10f;
	public float jumpMultiplier = 5f;
	public float initialJumpForce = 10f;
	public float jumpInputDuration = 1f;

	public float maxFallSpeed = 50f;

	private float gravityCancel = 1f;
	private bool isJumping = false;
	private float jumpBeginTime = -Mathf.Infinity;
	private float verticalMovement = 0f;
	private Vector3 finalMovementVector;
	private Vector3 movementVector;

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
		finalMovementVector = movementVector * speed;
		
		gravityCancel += Time.deltaTime * 10f;

		//Jump input timeout
		if(isJumping)
		{
			gravityCancel *= .5f;
			//verticalMovement = verticalMovement + (gravityCancel * Time.deltaTime);

			if(Time.time > jumpBeginTime + jumpInputDuration)
			{
				isJumping = false;
				gravityCancel = 1f;
			}
		}

		gravityCancel = Mathf.Clamp01(gravityCancel);
		verticalMovement = verticalMovement + (Physics.gravity.y * gravityMultiplier * Time.deltaTime * gravityCancel); //Add gravity contribution

		verticalMovement = Mathf.Clamp(verticalMovement, -maxFallSpeed, 100f);
		finalMovementVector.y = verticalMovement;

		characterController.Move(finalMovementVector * Time.deltaTime);

		//Rotate to the movement direction
		if(movementVector.sqrMagnitude >= .02f)
		{
			transform.forward = movementVector.normalized;
		}
	}

	private void OnMove(Vector2 movement)
	{
		movementVector = new Vector3(movement.x, 0f, movement.y);
	}

	private void OnJumpInitiated()
	{
		if(characterController.isGrounded)
		{
			isJumping = true;
			jumpBeginTime = Time.time;
			verticalMovement = initialJumpForce;
			gravityCancel = 0f;
		}
	}

	private void OnJumpCanceled()
	{
		isJumping = false;
	}
}