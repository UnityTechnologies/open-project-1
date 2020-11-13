using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IMenusActions
{
	public event UnityAction jumpEvent;
	public event UnityAction jumpCanceledEvent;
	public event UnityAction attackEvent;
	public event UnityAction interactEvent;
	public event UnityAction extraActionEvent;
	public event UnityAction pauseEvent;
	public event UnityAction<Vector2> moveEvent;
	public event UnityAction<Vector2, bool> cameraMoveEvent;
	public event UnityAction enableMouseControlCameraEvent;
	public event UnityAction disableMouseControlCameraEvent;

	// MenuEvents
	public event UnityAction<Vector2> MoveSelectionMenuEvent = delegate { };
	public event UnityAction MouseMoveMenuEvent = delegate { };
	public event UnityAction ConfirmMenuEvent = delegate { };
	public event UnityAction CancelMenuEvent = delegate { };
	public event UnityAction CloseMenuEvent = delegate { };


	public GameInput gameInput;

	private void OnEnable()
	{
		if (gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Gameplay.SetCallbacks(this);
			gameInput.Menus.SetCallbacks(this);
		}

		EnableGameplayInput();
	}

	private void OnDisable()
	{
		DisableControls();
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
			MoveSelectionMenuEvent(context.ReadValue<Vector2>());
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

	public void OnCloseMenu(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			CloseMenuEvent();
	}

	public void EnableMenuInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.Menus.Enable();
	}
	public void EnableGameplayInput()
	{
		gameInput.Gameplay.Enable();
		gameInput.Menus.Disable();
	}

	public void DisableControls()
	{
		gameInput.Gameplay.Disable();
		gameInput.Menus.Disable();
	}
}
