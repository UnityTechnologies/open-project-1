using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : MonoBehaviour
{
	public InputReader inputReader;
	public Transform gameplayCamera;

	private Character charScript;
	private bool controlsEnabled = true;

	private void Awake()
	{
		charScript = GetComponent<Character>();
	}

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
		inputReader.jumpEvent -= OnJumpInitiated;
		inputReader.jumpCanceledEvent -= OnJumpCanceled;
		inputReader.moveEvent -= OnMove;
		//...
	}

	//---- EVENT LISTENERS ----

	private void OnMove(Vector2 movement)
	{
		if(controlsEnabled)
		{
			//Get the two axes from the camera and flatten them on the XZ plane
			Vector3 cameraForward = gameplayCamera.forward;
			cameraForward.y = 0f;
			Vector3 cameraRight = gameplayCamera.right;
			cameraRight.y = 0f;

			//Use the two axes, modulated by the corresponding inputs, and construct the final vector
			Vector3 adjustedMovement = cameraRight.normalized * movement.x + cameraForward.normalized * movement.y;

			charScript.Move(Vector3.ClampMagnitude(adjustedMovement, 1f));
		}
	}

	private void OnJumpInitiated()
	{
		if(controlsEnabled) charScript.Jump();
	}

	private void OnJumpCanceled()
	{
		if(controlsEnabled) charScript.CancelJump();
	}
}
