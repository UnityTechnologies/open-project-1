using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menus
{
	public class SelectableUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		private MenuInput _menuInput;
		[SerializeField] private InputReader _inputReader;

		private void Awake()
		{
			_menuInput = transform.root.gameObject.GetComponentInChildren<MenuInput>();
		}

		private void OnEnable()
		{
			// _inputReader.MoveSelectionMenuEvent += InputReaderOnMoveSelectionMenuEvent;
		}

		private void OnDisable()
		{
			// _inputReader.MoveSelectionMenuEvent -= InputReaderOnMoveSelectionMenuEvent;
		}

		private void InputReaderOnMoveSelectionMenuEvent(Vector2 arg0)
		{
			_menuInput.HandleMouseExit(gameObject);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_menuInput.HandleMouseEnter(gameObject);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_menuInput.HandleMouseExit(gameObject);
		}
	}
}
