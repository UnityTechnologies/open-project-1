using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Events;

public class UIInventoryActionButton : MonoBehaviour
{

	[SerializeField]
	private LocalizeStringEvent _buttonActionText = default;

	[SerializeField]
	private Button _buttonAction = default;

	[SerializeField]
	UIButtonPrompt buttonPromptSetter = default;

	[SerializeField]
	InputReader _inputReader = default;

	bool hasEvent = false;

	public UnityAction Clicked;

	public void FillInventoryButton(ItemType itemType, bool isInteractable = true)
	{
		_buttonAction.interactable = isInteractable;
		_buttonActionText.StringReference = itemType.ActionName;
		//bool isKeyboard = !(Input.GetJoystickNames() != null && Input.GetJoystickNames().Length > 0);

		bool isKeyboard = true;
		buttonPromptSetter.SetButtonPrompt(isKeyboard);
		if (isInteractable)
		{

			if (_inputReader != null)
			{
				hasEvent = true;
				_inputReader.inventoryActionButtonEvent += ClickActionButton;
			}
		}
		else
		{
			if (_inputReader != null)
				if (hasEvent)
					_inputReader.inventoryActionButtonEvent -= ClickActionButton;

		}

	}
	public void ClickActionButton()
	{
		Clicked.Invoke();


	}
	private void OnDisable()
	{
		if (_inputReader != null)
			if (hasEvent)
				_inputReader.inventoryActionButtonEvent -= ClickActionButton;

	}


}
