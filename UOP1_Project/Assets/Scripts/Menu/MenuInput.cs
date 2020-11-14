using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInput : MonoBehaviour
{
	[SerializeField] private GameObject _currentSelection;
	[SerializeField] private GameObject _mouseSelection;
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

	/// <summary>
	/// Fired by keyboard and gamepad inputs.  current selected UI element will be the ui Element that was selected
	/// when the event was fired.  The _currentSelection is updated later on, after the EventSystem moves to the
	/// desired UI element, the UI element will call into UpdateSelection()
	/// </summary>
	private void HandleMoveSelection()
	{
		Cursor.visible = false;

		// Handle case where no UI element is selected because mouse left selectable bounds
		if (EventSystem.current.currentSelectedGameObject == null)
			EventSystem.current.SetSelectedGameObject(_currentSelection);

		// occurs when mouse is on top of some button, and we hit a gamepad or keyboard key to change the selection
		var mouseIsOverSelectionStart = _mouseSelection == _currentSelection;

		if (mouseIsOverSelectionStart)
		{
			// fire pointer exit event because we don't want the button to be in the 'highlighted' state
			ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject,
				new PointerEventData(EventSystem.current),
				ExecuteEvents.pointerExitHandler);
			// update the selection
			EventSystem.current.SetSelectedGameObject(_currentSelection);

			// the event we fired earlier clears _mouseSelection, reset it here because our cursor is still over the button
			_mouseSelection = _currentSelection;
		}
	}

	private void HandleMoveCursor()
	{
		if (_mouseSelection != null)
		{
			EventSystem.current.SetSelectedGameObject(_mouseSelection);
		}

		Cursor.visible = true;
	}

	public void HandleMouseEnter(GameObject uiElement)
	{
		_mouseSelection = uiElement;
		EventSystem.current.SetSelectedGameObject(uiElement);
	}

	public void HandleMouseExit(GameObject uiElement)
	{
		if (EventSystem.current.currentSelectedGameObject != uiElement) return;

		// deselect UI element if mouse moves away from it
		_mouseSelection = null;
		EventSystem.current.SetSelectedGameObject(null);
	}

	/// <summary>
	/// Fired by gamepad or keyboard navigation inputs
	/// </summary>
	/// <param name="uiElement"></param>
	public void UpdateSelection(GameObject uiElement) => _currentSelection = uiElement;

	private void OnGUI()
	{
		GUILayout.Box($"_currentSelection: {(_currentSelection != null ? _currentSelection.name : "null")}");
		GUILayout.Box($"_mouseSelection: {(_mouseSelection != null ? _mouseSelection.name : "null")}");
	}
}
