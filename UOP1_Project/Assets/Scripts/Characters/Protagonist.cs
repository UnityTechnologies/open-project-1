using UnityEngine;

public class Protagonist : MonoBehaviour
{
    public InputReader inputReader;
    public Transform gameplayCamera;

    private Character charScript;
    private Vector2 previousMovementInput;
    private bool controlsEnabled = true;

    public void Initialise(InputReader inputReader, Transform gameplayCamera)
    {
        this.inputReader = inputReader;
        this.gameplayCamera = gameplayCamera;

        EnableInputs();
    }

    private void Awake()
    {
        charScript = GetComponent<Character>();
    }

    //Adds listeners for events being triggered in the InputReader script
    private void OnEnable()
    {
        if (inputReader != null)
        {
            EnableInputs();
        }
    }

    //Removes all listeners to the events coming from the InputReader script
    private void OnDisable()
    {
        if (inputReader != null)
        {
            DisableInputs();
        }
    }

    private void EnableInputs()
    {
        inputReader.jumpEvent += OnJumpInitiated;
        inputReader.jumpCanceledEvent += OnJumpCanceled;
        inputReader.moveEvent += OnMove;
        //...
    }

    private void DisableInputs()
    {
        inputReader.jumpEvent -= OnJumpInitiated;
        inputReader.jumpCanceledEvent -= OnJumpCanceled;
        inputReader.moveEvent -= OnMove;
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
        Vector3 adjustedMovement = cameraRight.normalized * previousMovementInput.x +
            cameraForward.normalized * previousMovementInput.y;

        charScript.Move(Vector3.ClampMagnitude(adjustedMovement, 1f));
    }

    //---- EVENT LISTENERS ----

    private void OnMove(Vector2 movement)
    {
        if (controlsEnabled) previousMovementInput = movement;
    }

    private void OnJumpInitiated()
    {
        if (controlsEnabled) charScript.Jump();
    }

    private void OnJumpCanceled()
    {
        if (controlsEnabled) charScript.CancelJump();
    }
}
