using UnityEngine;

/// <summary>
/// <para>This class listens to the input and it deposits it on the <c>Character</c> component, ready to be used by the <c>StateMachine</c></para>
/// </summary>
public class Protagonist : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader = default;
	public Transform gameplayCamera;

	private Protagonist _charScript;
	private Vector2 _previousMovementInput;

	//These fields are manipulated by the StateMachine actions
	[HideInInspector] public bool jumpInput;
	[HideInInspector] public Vector3 movementInput; //Initial input coming from the Protagonist script
	[HideInInspector] public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions
	[HideInInspector] public ControllerColliderHit lastHit;

	private void Awake()
	{
		_charScript = GetComponent<Protagonist>();
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
		//...
	}

	//Removes all listeners to the events coming from the InputReader script
	private void OnDisable()
	{
		_inputReader.jumpEvent -= OnJumpInitiated;
		_inputReader.jumpCanceledEvent -= OnJumpCanceled;
		_inputReader.moveEvent -= OnMove;
		//...
	}

	private void Update()
	{
		RecalculateMovement();
	}

	private void RecalculateMovement()
	{
		//Get the two axes from the camera and flatten them on the XZ plane
		Vector3 cameraForward = gameplayCamera.forward;
		cameraForward.y = 0f;
		Vector3 cameraRight = gameplayCamera.right;
		cameraRight.y = 0f;

		//Use the two axes, modulated by the corresponding inputs, and construct the final vector
		Vector3 adjustedMovement = cameraRight.normalized * _previousMovementInput.x +
			cameraForward.normalized * _previousMovementInput.y;

		movementInput = Vector3.ClampMagnitude(adjustedMovement, 1f);
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
}
