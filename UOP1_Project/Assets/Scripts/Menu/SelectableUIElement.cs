using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
	private MenuInput _menuInput;

	private void Awake()
	{
		_menuInput = transform.root.gameObject.GetComponentInChildren<MenuInput>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_menuInput.HandleMouseEnter(gameObject);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_menuInput.HandleMouseExit(gameObject);
	}

	public void OnSelect(BaseEventData eventData)
	{
		_menuInput.UpdateSelection(gameObject);
	}
}
