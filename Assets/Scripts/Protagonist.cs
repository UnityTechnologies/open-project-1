using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : MonoBehaviour
{
	public InputReader inputReader;

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

	private void OnMove(Vector2 movement)
	{
		Debug.Log("Move " + movement);
	}

	private void OnJump()
   {
	   Debug.Log("JUMP");
   }
}
