using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IDialoguesActions, GameInput.IMenusActions
{
	// Gameplay
	public event UnityAction jumpEvent;
	public event UnityAction jumpCanceledEvent;
	public event UnityAction attackEvent;
	public event UnityAction interactEvent; // Used to talk, pickup objects, interact with tools like the cooking cauldron
	public event UnityAction extraActionEvent; // Used to bring up the inventory
	public event UnityAction pauseEvent;
	public event UnityAction<Vector2> moveEvent;
	public event UnityAction<Vector2, bool> cameraMoveEvent;
	public event UnityAction enableMouseControlCameraEvent;
	public event UnityAction disableMouseControlCameraEvent;
	public event UnityAction startedRunning;
	public event UnityAction stoppedRunning;

	// Dialogue
	public event UnityAction advanceDialogueEvent;
	public event UnityAction onMoveSelectionEvent;

	// MenuEvents
	public event UnityAction MoveSelectionMenuEvent = delegate { };
	public event UnityAction MouseMoveMenuEvent = delegate { };
	public event UnityAction ConfirmMenuEvent = delegate { };
	public event UnityAction CancelMenuEvent = delegate { };
	public event UnityAction UnpauseMenuEvent = delegate { };


	private GameInput gameInput;

	private void OnEnable()
	{
		if (gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Menus.SetCallbacks(this);
			gameInput.Gameplay.SetCallbacks(this);
			gameInput.Dialogues.SetCallbacks(this);
		}

		EnableGameplayInput();
	}

	private void OnDisable()
	{
		DisableAllInput();
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		if (attackEvent != null
			&& context.phase == InputActionPhase.Performed)
			attackEvent.Invoke();
	}

	public void OnExtraAction(InputAction.CallbackContext context)
	{
		if (extraActionEvent != null
			&& context.phase == InputActionPhase.Performed)
			extraActionEvent.Invoke();
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if (interactEvent != null
			&& context.phase == InputActionPhase.Performed)
			interactEvent.Invoke();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (jumpEvent != null
			&& context.phase == InputActionPhase.Performed)
			jumpEvent.Invoke();

		if (jumpCanceledEvent != null
			&& context.phase == InputActionPhase.Canceled)
			jumpCanceledEvent.Invoke();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if (moveEvent != null)
		{
			moveEvent.Invoke(context.ReadValue<Vector2>());
		}
	}

	public void OnRun(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				startedRunning?.Invoke();
				break;
			case InputActionPhase.Canceled:
				stoppedRunning?.Invoke();
				break;
		}
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		if (pauseEvent != null
			&& context.phase == InputActionPhase.Performed)
			pauseEvent.Invoke();
	}

	public void OnRotateCamera(InputAction.CallbackContext context)
	{
		if (cameraMoveEvent != null)
		{
			cameraMoveEvent.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
		}
	}

	public void OnMouseControlCamera(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			enableMouseControlCameraEvent?.Invoke();

		if (context.phase == InputActionPhase.Canceled)
			disableMouseControlCameraEvent?.Invoke();
	}

	private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

	public void OnMoveSelection(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			onMoveSelectionEvent();
	}

	public void OnAdvanceDialogue(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			advanceDialogueEvent();
	}

	public void OnMoveMenuSelection(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			MoveSelectionMenuEvent();
	}

	public void OnConfirm(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			ConfirmMenuEvent();
	}

	public void OnCancel(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			CancelMenuEvent();
	}

	public void OnMouseMove(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			MouseMoveMenuEvent();
	}

	public void OnUnpause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			UnpauseMenuEvent();
	}

	public void EnableDialogueInput()
	{
		gameInput.Dialogues.Enable();
		gameInput.Menus.Disable();
		gameInput.Gameplay.Disable();
		gameInput.Menus.Disable();
	}

	public void EnableGameplayInput()
	{
		gameInput.Gameplay.Enable();
		gameInput.Menus.Disable();
		gameInput.Dialogues.Disable();
		gameInput.Menus.Disable();
	}

	public void EnableUIInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.Dialogues.Disable();
		gameInput.Menus.Enable();
	}

	public void DisableAllInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.Menus.Disable();
		gameInput.Dialogues.Disable();
	}

	public void EnableMenuInput()
	{
		gameInput.Dialogues.Disable();
		gameInput.Gameplay.Disable();
		gameInput.Menus.Enable();
	}

	public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
}
