using UnityEngine.EventSystems;
using UnityEngine.UI;

// Chop Chop button
public class CC_Button : Button
{
	private MenuSelectionHandler _menuSelectionHandler;

	private void Awake()
	{
		_menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		_menuSelectionHandler.HandleMouseEnter(gameObject);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		_menuSelectionHandler.HandleMouseExit(gameObject);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		_menuSelectionHandler.UpdateSelection(gameObject);
		base.OnSelect(eventData);
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		if (_menuSelectionHandler.AllowsSubmitOccurance())
			base.OnSubmit(eventData);
	}
}
