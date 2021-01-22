﻿using System;
using UnityEngine;

/// <summary>
/// <para>This component consumes input on the InputReader and stores its values. The input is then read, and manipulated, by the StateMachines's Actions.</para>
/// </summary>
public class Protagonist : MonoBehaviour, IAttackerCharacter
{
	[SerializeField] private InputReader _inputReader = default;
	public TransformAnchor gameplayCameraTransform;

	[SerializeField] private VoidEventChannelSO _openInventoryChannel = default;

	private Vector2 _previousMovementInput;

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

	[SerializeField]
	private HealthAnchor _healthAnchor = default;

	[SerializeField]
	private AttackConfigSO _attackConfigSO;
	private AttackData _attackData;


	public bool getHit { get; set; }
	public bool isDead { get => _healthAnchor.isDead(); }

	private void Awake()
	{
		_attackData = _attackConfigSO.createAttackData();
		_healthAnchor.FillHealth();
	}

	public AttackConfigSO getAttackConfig()
	{
		return _attackConfigSO;
	}

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
		_inputReader.openInventoryEvent += OnOpenInventory;
		_inputReader.startedRunning += OnStartedRunning;
		_inputReader.stoppedRunning += OnStoppedRunning;
		_inputReader.attackEvent += OnAttack;
		//...
	}

	//Removes all listeners to the events coming from the InputReader script
	private void OnDisable()
	{
		_inputReader.jumpEvent -= OnJumpInitiated;
		_inputReader.jumpCanceledEvent -= OnJumpCanceled;
		_inputReader.moveEvent -= OnMove;
		_inputReader.openInventoryEvent -= OnOpenInventory;
		_inputReader.startedRunning -= OnStartedRunning;
		_inputReader.stoppedRunning -= OnStoppedRunning;
		//...
	}

	private void Update()
	{
		RecalculateMovement();
		_attackConfigSO.computeAttackCapability(_attackData);
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

		// This is used to set the speed to the maximum if holding the Shift key,
		// to allow keyboard players to "run"
		if (isRunning)
			movementInput.Normalize();
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

	private void OnStoppedRunning() => isRunning = false;

	private void OnStartedRunning() => isRunning = true;

	private void OnOpenInventory()
	{
		_openInventoryChannel.RaiseEvent();
	}

	private void OnAttack() => attackInput = true;

	private void OnTriggerEnter(Collider other)
	{
		CritterWeapon critterWeapon = other.GetComponent<CritterWeapon>();
		if (!getHit && critterWeapon != null && critterWeapon.Enable)
		{
			ReceiveAnAttack(critterWeapon.AttackStrength);
		}
	}

	private void ReceiveAnAttack(int damage)
	{
		_healthAnchor.DecreaseHealth(damage);
		getHit = true;
	}

	public void TriggerAttack()
	{
		_attackConfigSO.Attack(_attackData);
	}

	public bool IsReadyToAttack()
	{
		return _attackConfigSO.isReadyToAttack(_attackData);
	}

}
