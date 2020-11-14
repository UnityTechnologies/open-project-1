using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MenuInputReader: GameInput.IMenusActions
{
	public MenuInputReader(GameInput gameInput)
	{
		gameInput.Menus.SetCallbacks(this);
	}

	// MenuEvents
	public event UnityAction MoveSelectionMenuEvent = delegate { };
	public event UnityAction MouseMoveMenuEvent = delegate { };
	public event UnityAction ConfirmMenuEvent = delegate { };
	public event UnityAction CancelMenuEvent = delegate { };
	public event UnityAction CloseMenuEvent = delegate { };
	public void OnMoveSelection(InputAction.CallbackContext context)
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

	public void OnCloseMenu(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			CloseMenuEvent();
	}
}
