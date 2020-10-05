using UnityEngine;

namespace Assets.Scripts.Characters
{
	public class Protagonist : MonoBehaviour
	{
		public InputReader inputReader;
		public Transform gameplayCamera;

		private readonly bool controlsEnabled = true;
		private Character charScript;
		private Vector2 previousMovementInput;

		private void Awake()
		{
			charScript = GetComponent<Character>();
		}

		// Adds listeners for events being triggered in the InputReader script
		private void OnEnable()
		{
			inputReader.jumpEvent += OnJumpInitiated;
			inputReader.jumpCanceledEvent += OnJumpCanceled;
			inputReader.moveEvent += OnMove;

			// ...
		}

		// Removes all listeners to the events coming from the InputReader script
		private void OnDisable()
		{
			inputReader.jumpEvent -= OnJumpInitiated;
			inputReader.jumpCanceledEvent -= OnJumpCanceled;
			inputReader.moveEvent -= OnMove;

			// ...
		}

		private void Update()
		{
			RecalculateMovement();
		}

		private void RecalculateMovement()
		{
			// Get the two axes from the camera and flatten them on the XZ plane
			Vector3 cameraForward = gameplayCamera.forward;
			cameraForward.y = 0f;
			Vector3 cameraRight = gameplayCamera.right;
			cameraRight.y = 0f;

			// Use the two axes, modulated by the corresponding inputs, and construct the final vector
			Vector3 adjustedMovement = (cameraRight.normalized * previousMovementInput.x) +
				(cameraForward.normalized * previousMovementInput.y);

			charScript.Move(Vector3.ClampMagnitude(adjustedMovement, 1f));
		}

		#region  EVENT LISTENERS

		private void OnMove(Vector2 movement)
		{
			if (controlsEnabled)
			{
				previousMovementInput = movement;
			}
		}

		private void OnJumpInitiated()
		{
			if (controlsEnabled)
			{
				charScript.Jump();
			}
		}

		private void OnJumpCanceled()
		{
			if (controlsEnabled)
			{
				charScript.CancelJump();
			}
		}

		#endregion
	}
}
