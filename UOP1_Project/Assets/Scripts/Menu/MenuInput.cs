using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInput : MonoBehaviour
{
	private GameObject _currentSelection;
	private GameObject _mouseSelection;
	[SerializeField] private InputReader _inputReader;
	public bool IsMouseActive { get; private set; }
	private void OnEnable()
	{
		_inputReader.MouseMoveMenuEvent += HandleMoveCursor;
		_inputReader.MoveSelectionMenuEvent += HandleMoveSelection;
	}

	private void OnDisable()
	{
		_inputReader.MouseMoveMenuEvent -= HandleMoveCursor;
		_inputReader.MoveSelectionMenuEvent -= HandleMoveSelection;
	}

	private void HandleMoveSelection(Vector2 dir)
	{
		DisableCursor();

		// occurs when mouse is on top of some button, and we hit a gamepad or keyboard key to change the selection
		var exitingMouseMode = EventSystem.current.currentSelectedGameObject == _currentSelection;

		if (exitingMouseMode)
		{
			ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current),
				ExecuteEvents.pointerExitHandler);
			EventSystem.current.SetSelectedGameObject(_currentSelection);
		}

		if (EventSystem.current.currentSelectedGameObject == null)
			EventSystem.current.SetSelectedGameObject(_mouseSelection);
	}

	private void DisableCursor()
	{
		Cursor.visible = false;
		IsMouseActive = false;
	}

	private void HandleMoveCursor()
	{
		EnableCursor();
	}

	private void EnableCursor()
	{
		Cursor.visible = true;
		IsMouseActive = true;
	}

	private void StoreSelection(GameObject uiElement)
	{
		EventSystem.current.SetSelectedGameObject(uiElement);
		_currentSelection = uiElement;
	}

	public void HandleMouseEnter(GameObject uiElement)
	{
		StoreSelection(uiElement);
	}
	public void HandleMouseExit(GameObject uiElement)
	{
		if (EventSystem.current.currentSelectedGameObject == uiElement)
		{
			_mouseSelection = uiElement;
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
}
