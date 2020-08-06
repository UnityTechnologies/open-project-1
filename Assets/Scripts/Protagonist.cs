using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : MonoBehaviour
{
	public InputReader inputReader;
	private Rigidbody rigidbody;

	public float speed = 10f;
	public float airMomentumMultiplier = 3f;
	public float jumpStrength = 1f;
	private Vector3 momentum;
	private bool isGrounded = true;
	private bool jumpInitiated = false; //Becomes true on the frame that the jump button is pressed, consumed in FixedUpdate

	//Adds listeners for events being triggered in the InputReader script
	private void OnEnable()
	{
		inputReader.jumpEvent += OnJump;
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
		rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (isGrounded)
		{
			if(jumpInitiated)
			{
				//Jump!
				//rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
				rigidbody.velocity = new Vector3(momentum.x * speed, jumpStrength, momentum.z * speed);
				jumpInitiated = false;
				isGrounded = false;
			}
			else
			{
				//Normal movement
				rigidbody.MovePosition(rigidbody.position + momentum * Time.fixedDeltaTime * speed);
			}
		}
		else
		{
			//In midair
			rigidbody.AddForce(momentum * airMomentumMultiplier * Time.fixedDeltaTime * speed);
		}
	}

	private void OnMove(Vector2 movement)
	{
		Debug.Log("Move " + movement);
		momentum = new Vector3(movement.x, 0f, movement.y);
	}

	private void OnJump()
	{
		if (isGrounded)
		{
			jumpInitiated = true;
		}
	}

	private void OnCollisionEnter(Collision coll)
	{
		isGrounded = true;
		rigidbody.velocity = Vector3.zero;
	}
}