using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// <para>This class is a data holder that the <c>StateMachine</c> class uses to deposit data that needs to be shared between states.
/// Ideally, both the player character and NPCs can use this component to drive locomotion.</para>
/// <para>Also used to listen to the native Unity Message <c>OnControllerColliderHit</c> from the <c>CharacterController</c> component,
/// which requires a <c>MonoBehaviour</c> to be listened to.</para>
/// </summary>
public class Character : MonoBehaviour
{
	//These fields are manipulated by the StateMachine actions
	[HideInInspector] public bool jumpInput;
	[HideInInspector] public Vector3 movementInput; //Initial input coming from the Protagonist script
	[HideInInspector] public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions
	[HideInInspector] public ControllerColliderHit lastHit;

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
