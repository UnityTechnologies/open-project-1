using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Events;

public class UIActionButton : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _buttonActionText = default;
	[SerializeField] private Button _buttonAction = default;
	[SerializeField] private UIButtonPrompt _buttonPromptSetter = default;
	[SerializeField] private InputReader _inputReader = default;

	public UnityAction Clicked;

	bool _hasEvent = false;

	public void FillInventoryButton(ItemTypeSO itemType, bool isInteractable = true)
	{
		_buttonAction.interactable = isInteractable;
		_buttonActionText.StringReference = itemType.ActionName;

		bool isKeyboard = true;
		_buttonPromptSetter.SetButtonPrompt(isKeyboard);
		if (isInteractable)
		{
			if (_inputReader != null)
			{
				_hasEvent = true;
				_inputReader.InventoryActionButtonEvent += ClickActionButton;
			}
		}
		else
		{
			if (_inputReader != null)
				if (_hasEvent)
					_inputReader.InventoryActionButtonEvent -= ClickActionButton;
		}
	}

	public void ClickActionButton()
	{
		Clicked.Invoke();
	}

	private void OnDisable()
	{
		if (_inputReader != null)
			if (_hasEvent)
				_inputReader.InventoryActionButtonEvent -= ClickActionButton;
	}
}