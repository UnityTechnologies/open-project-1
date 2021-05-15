using System;
using UnityEngine;
using static EventAggregator;

/// <summary>
/// <para>This component consumes input on the InputReader and stores its values. The input is then read, and manipulated, by the StateMachines's Actions.</para>
/// </summary>
public class Protagonist : MonoBehaviour,
	IHandle<AttackEvent>,
	IHandle<JumpEvent>,
	IHandle<JumpCancelledEvent>,
	IHandle<MoveEvent>,
	IHandle<StartedRunningEvent>,
	IHandle<StoppedRunningEvent>,
	IHandle<OpenInventoryEvent>
{
	[SerializeField] private EventAggregatorSO _eventAggregator;

	public TransformAnchor gameplayCameraTransform;

	[SerializeField] private VoidEventChannelSO _openInventoryChannel = default;

	private Vector2 _inputVector;
	private float _previousSpeed;

	//These fields are read and manipulated by the StateMachine actions
	[NonSerialized] public bool jumpInput;
	[NonSerialized] public bool extraActionInput;
	[NonSerialized] public bool attackInput;
	[NonSerialized] public Vector3 movementInput; //Initial input coming from the Protagonist script
	[NonSerialized] public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions
	[NonSerialized] public ControllerColliderHit lastHit;
	[NonSerialized] public bool isRunning; // Used when using the keyboard to run, brings the normalised speed to 1

	public const float GRAVITY_MULTIPLIER = 5f;
	public const float MAX_FALL_SPEED = -50f;
	public const float MAX_RISE_SPEED = 100f;
	public const float GRAVITY_COMEBACK_MULTIPLIER = .03f;
	public const float GRAVITY_DIVIDER = .6f;
	public const float AIR_RESISTANCE = 5f;

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		lastHit = hit;
	}

	//Adds listeners for events being triggered in the InputReader script
	private void OnEnable()
	{
		_eventAggregator.Subscribe(this);
	}

	//Removes all listeners to the events coming from the InputReader script
	private void OnDisable()
	{
		_eventAggregator.Unsubscribe(this);
	}

	private void Update()
	{
		RecalculateMovement();
	}

	private void RecalculateMovement()
	{
		float targetSpeed = 0f;
		Vector3 adjustedMovement;

		if (gameplayCameraTransform.isSet)
		{
			//Get the two axes from the camera and flatten them on the XZ plane
			Vector3 cameraForward = gameplayCameraTransform.Transform.forward;
			cameraForward.y = 0f;
			Vector3 cameraRight = gameplayCameraTransform.Transform.right;
			cameraRight.y = 0f;

			//Use the two axes, modulated by the corresponding inputs, and construct the final vector
			adjustedMovement = cameraRight.normalized * _inputVector.x +
				cameraForward.normalized * _inputVector.y;
		}
		else
		{
			//No CameraManager exists in the scene, so the input is just used absolute in world-space
			Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
			adjustedMovement = new Vector3(_inputVector.x, 0f, _inputVector.y);
		}

		//Fix to avoid getting a Vector3.zero vector, which would result in the player turning to x:0, z:0
		if (_inputVector.sqrMagnitude == 0f)
			adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

		//Accelerate/decelerate
		targetSpeed = Mathf.Clamp01(_inputVector.magnitude);
		if (targetSpeed > 0f)
		{
			// This is used to set the speed to the maximum if holding the Shift key,
			// to allow keyboard players to "run"
			if(isRunning)
				targetSpeed = 1f;

			if (attackInput)
				targetSpeed = .05f;
		}
		targetSpeed = Mathf.Lerp(_previousSpeed, targetSpeed, Time.deltaTime * 4f);

		movementInput = adjustedMovement.normalized * targetSpeed;

		_previousSpeed = targetSpeed;
	}

	// Triggered from Animation Event
	public void ConsumeAttackInput() => attackInput = false;

	public void Handle(AttackEvent msg)
	{
		attackInput = true;
	}

	public void Handle(JumpEvent msg)
	{
		jumpInput = true;
	}

	public void Handle(JumpCancelledEvent msg)
	{
		jumpInput = false;
	}

	public void Handle(MoveEvent msg)
	{
		_inputVector = msg.Movement;
	}

	public void Handle(StartedRunningEvent msg)
	{
		isRunning = true;
	}

	public void Handle(StoppedRunningEvent msg)
	{
		isRunning = false;
	}

	public void Handle(OpenInventoryEvent msg)
	{	// Clearly this event should be handled elsewhere and not on the
		// protagonist but for now limited changes.
		_openInventoryChannel.RaiseEvent();
	}
}
