using System;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/SelectableUIElement")]
public class SelectableUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
	private MenuSelectionHandler _menuInput;

	private void Awake()
	{
		_menuInput = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
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
