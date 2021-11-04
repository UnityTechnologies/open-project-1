using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("UOP1/UI/MultiInputSelectableElement")]
public class MultiInputSelectableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
	private MenuSelectionHandler _menuSelectionHandler;

	private void Awake()
	{
		_menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_menuSelectionHandler.HandleMouseEnter(gameObject);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_menuSelectionHandler.HandleMouseExit(gameObject);
	}

	public void OnSelect(BaseEventData eventData)
	{
		_menuSelectionHandler.UpdateSelection(gameObject);
	}
}
