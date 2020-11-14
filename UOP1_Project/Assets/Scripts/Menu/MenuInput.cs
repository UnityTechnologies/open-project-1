using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInput : MonoBehaviour
{
	private GameObject _currentSelection;
	private GameObject _mouseSelection;
	[SerializeField] private InputReader _inputReader;
	private void OnEnable()
	{
		_inputReader.Menu.MouseMoveMenuEvent += HandleMoveCursor;
		_inputReader.Menu.MoveSelectionMenuEvent += HandleMoveSelection;
	}

	private void OnDisable()
	{
		_inputReader.Menu.MouseMoveMenuEvent -= HandleMoveCursor;
		_inputReader.Menu.MoveSelectionMenuEvent -= HandleMoveSelection;
	}

	private void HandleMoveSelection()
	{
		// _currentSelection will be the start UI element, destination is taken care of by the EventSystem for us

		DisableCursor();

		// occurs when mouse is on top of some button, and we hit a gamepad or keyboard key to change the selection
		var mouseIsOverSelectionStart = _mouseSelection == _currentSelection;

		if (mouseIsOverSelectionStart)
		{
			// fire pointer exit event because we don't want the button to be in the 'highlighted' state
			ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current),
				ExecuteEvents.pointerExitHandler);
		}

		// if mouse has moved from a button to empty space we store its last interactable
		// if we receive a move command, we use that stored position to recenter focus before the move is executed
		if (EventSystem.current.currentSelectedGameObject == null)
			EventSystem.current.SetSelectedGameObject(_mouseSelection);
	}

	private void DisableCursor()
	{
		Cursor.visible = false;
	}

	private void HandleMoveCursor()
	{
		EnableCursor();
	}

	private void EnableCursor()
	{
		Cursor.visible = true;
	}

	private void StoreSelection(GameObject uiElement)
	{
		EventSystem.current.SetSelectedGameObject(uiElement);
		_currentSelection = uiElement;
		_mouseSelection = uiElement;
	}

	public void HandleMouseEnter(GameObject uiElement)
	{
		StoreSelection(uiElement);
	}

	public void HandleMouseExit(GameObject uiElement)
	{
		// deselect UI element if mouse moves away from it
		if (EventSystem.current.currentSelectedGameObject == uiElement)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
}
