using UnityEngine;
using UnityEngine.PlayerLoop;

public class Character : MonoBehaviour
{
	public bool jumpInput;
	public Vector3 movementInput; //Initial input coming from the Protagonist script
	public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions
	public ControllerColliderHit lastHit;

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		lastHit = hit;
	}

	//---- COMMANDS ISSUED BY OTHER SCRIPTS ----
	public void Move(Vector3 movement)
	{
		movementInput = movement;
	}

	public void Jump()
	{
		jumpInput = true;
	}

	public void CancelJump()
	{
		jumpInput = false;
	}
}
