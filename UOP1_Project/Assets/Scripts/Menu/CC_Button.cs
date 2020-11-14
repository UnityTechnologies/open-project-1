using UnityEngine.EventSystems;
using UnityEngine.UI;

// Chop Chop button
public class CC_Button : Button
{
	private MenuInput _menuInput;

	private void Awake()
	{
		_menuInput = transform.root.gameObject.GetComponentInChildren<MenuInput>();
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		_menuInput.HandleMouseEnter(gameObject);
		base.OnPointerEnter(eventData);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		_menuInput.HandleMouseExit(gameObject);
		base.OnPointerExit(eventData);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		_menuInput.UpdateSelection(gameObject);
		base.OnSelect(eventData);
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		if (_menuInput.AllowsSubmitOccurance(gameObject))
			base.OnSubmit(eventData);
	}
}
