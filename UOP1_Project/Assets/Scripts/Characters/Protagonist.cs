using System;
using UnityEngine;

/// <summary>
/// <para>This component consumes input on the InputReader and stores its values. The input is then read, and manipulated, by the StateMachines's Actions.</para>
/// </summary>
public class Protagonist : MonoBehaviour
{
	
	[SerializeField] private InputReader _inputReader = default;
	public TransformAnchor gameplayCameraTransform;

	private Vector2 _previousMovementInput;

	//These fields are read and manipulated by the StateMachine actions
	[HideInInspector] public bool jumpInput;
	[HideInInspector] public bool extraActionInput;
	[HideInInspector] public Vector3 movementInput; //Initial input coming from the Protagonist script
	[HideInInspector] public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions
	[HideInInspector] public ControllerColliderHit lastHit;

	[Header("Event Channels")]
	[SerializeField] private Vector3ArrayChannelSO _movePlayerChannel;
	[SerializeField] private TransformEventChannelSO _moveCameraChannel;

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		lastHit = hit;
	}

	//Adds listeners for events being triggered in the InputReader script
	private void OnEnable()
	{
		_inputReader.jumpEvent += OnJumpInitiated;
		_inputReader.jumpCanceledEvent += OnJumpCanceled;
		_inputReader.moveEvent += OnMove;
		_inputReader.extraActionEvent += OnExtraAction;
		if (_movePlayerChannel != null)
			_movePlayerChannel.OnEventRaised += Teleport;
		//...
	}

	//Removes all listeners to the events coming from the InputReader script
	private void OnDisable()
	{
		_inputReader.jumpEvent -= OnJumpInitiated;
		_inputReader.jumpCanceledEvent -= OnJumpCanceled;
		_inputReader.moveEvent -= OnMove;
		_inputReader.extraActionEvent -= OnExtraAction;
		if (_movePlayerChannel != null)
			_movePlayerChannel.OnEventRaised -= Teleport;
		//...
	}

	private void Update()
	{
		RecalculateMovement();
	}
	/// <summary>
	/// Teleports the protagonist to another location(and rotation).
	/// </summary>
	/// <param name="vectors">This Vector3 array must be of length 2. Where elements [0] and [1] are position and rotation respectively.</param>
	private void Teleport(Vector3[] vectors)
	{
		if (vectors.Length == 2)
		{
			this.transform.position = vectors[0];
			this.transform.rotation = Quaternion.Euler(vectors[1]);
			Physics.SyncTransforms();
		}
		
	}
	private void RecalculateMovement()
	{
		if (gameplayCameraTransform.isSet)
		{
			//Get the two axes from the camera and flatten them on the XZ plane
			Vector3 cameraForward = gameplayCameraTransform.Transform.forward;
			cameraForward.y = 0f;
			Vector3 cameraRight = gameplayCameraTransform.Transform.right;
			cameraRight.y = 0f;

			//Use the two axes, modulated by the corresponding inputs, and construct the final vector
			Vector3 adjustedMovement = cameraRight.normalized * _previousMovementInput.x +
				cameraForward.normalized * _previousMovementInput.y;

			movementInput = Vector3.ClampMagnitude(adjustedMovement, 1f);
		}
		else
		{
			//No CameraManager exists in the scene, so the input is just used absolute in world-space
			Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
			movementInput = new Vector3(_previousMovementInput.x, 0f, _previousMovementInput.y);
		}
	}

	//---- EVENT LISTENERS ----

	private void OnMove(Vector2 movement)
	{
		_previousMovementInput = movement;
	}

	private void OnJumpInitiated()
	{
		jumpInput = true;
	}

	private void OnJumpCanceled()
	{
		jumpInput = false;
	}

	// This handler is just used for debug, for now
	private void OnExtraAction()
	{
		extraActionInput = true;
	}
}
