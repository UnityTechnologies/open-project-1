using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTesting : MonoBehaviour, GameInput.IGameplayActions
{
	GameInput gameInput;
	
	private void OnEnable()
	{
		if(gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Gameplay.SetCallbacks(this);
		}
		gameInput.Gameplay.Enable();

		Debug.Log("I'm enabled");
	}

	private void OnDisable()
	{
		gameInput.Gameplay.Disable();
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Started)
			Debug.Log("Attack");
	}

	public void OnExtraAction(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Started)
			Debug.Log("ExtraAction");
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Started)
			Debug.Log("Interact");
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Started)
			Debug.Log("Jump");
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Performed)
			Debug.Log("Move " + context.ReadValue<Vector2>());
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Started)
			Debug.Log("Pause");
	}
}
