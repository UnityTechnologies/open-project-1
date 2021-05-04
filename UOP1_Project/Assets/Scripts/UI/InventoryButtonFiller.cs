using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class InventoryButtonFiller : MonoBehaviour
{

	[SerializeField]
	private LocalizeStringEvent _buttonActionText = default;

	[SerializeField]
	private Button _buttonAction = default;

	[SerializeField]
	UIButtonPromptSetter buttonPromptSetter = default;

	[SerializeField]
	InputReader _inputReader = default;


	[SerializeField]
	VoidEventChannelSO _inventoryActionEvent = default;

	bool hasEvent = false; 


	public void FillInventoryButtons(ItemType itemType, bool isInteractable = true)
	{
		_buttonAction.interactable = isInteractable;
		_buttonActionText.StringReference = itemType.ActionName;
		//bool isKeyboard = !(Input.GetJoystickNames() != null && Input.GetJoystickNames().Length > 0);

		bool isKeyboard =true;
		buttonPromptSetter.SetButtonPrompt(isKeyboard);
		if (isInteractable)
		{

			if (_inputReader != null)
			{
				hasEvent = true;
				_inputReader.inventoryActionButtonEvent += ActionButtonEventRaised;
			}
		}
		else
		{
			if (_inputReader != null)
				if (hasEvent )
				_inputReader.inventoryActionButtonEvent -= ActionButtonEventRaised;

		}

	}
	public void ActionButtonEventRaised()
	{
		if(_inventoryActionEvent!=null)
		{
			_inventoryActionEvent.RaiseEvent(); 
		}


	}
	private void OnDisable()
	{
		if (_inputReader != null)
			if (hasEvent)
				_inputReader.inventoryActionButtonEvent -= ActionButtonEventRaised;

	}


}
