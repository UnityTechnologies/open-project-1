using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UnityEngine.Localization;

public enum PopupButtonType{
	Confirm,
	Cancel,
	Close,
	DoNothing,
}
public enum PopupType
{
	Quit,
	NewGame, 
}
public class UIPopupSetter : MonoBehaviour
{


	[SerializeField] private LocalizeStringEvent _titleText = default;
	[SerializeField] private LocalizeStringEvent _descriptionText = default;

	[SerializeField] private Button _buttonClose = default;
	[SerializeField] private UIPopupButtonSetter _popupButton1 = default;
	[SerializeField] private UIPopupButtonSetter _popupButton2 = default;

	[SerializeField] private InputReader _inputReader = default;

	PopupType actualType;
	
	[SerializeField]
	private IntEventChannelSO _buttonClickedEvent=default;

	[SerializeField]
	private VoidEventChannelSO _closePopupEvent = default;

	[SerializeField]
	private BoolEventChannelSO _confirmPopupEvent = default;

	private void Start()
	{
		_buttonClose.onClick.RemoveAllListeners(); 
		_buttonClose.onClick.AddListener(() => {  ButtonClicked((int)PopupButtonType.Close); });
		_buttonClickedEvent.OnEventRaised += ButtonClicked; 
	}
	public void SetPopup(PopupType popupType)
	{
		actualType = popupType;
		bool isConfirmation = false;
		bool hasExitButton = false;
		_titleText.StringReference.TableEntryReference = actualType.ToString() + "_Popup_Title";
		_descriptionText.StringReference.TableEntryReference = actualType.ToString() + "_Popup_Description";
		
		switch (actualType)
		{
			case PopupType.NewGame:
				isConfirmation = true;
				_popupButton1.SetButton(PopupButtonType.Confirm, actualType);
				_popupButton2.SetButton(PopupButtonType.Cancel, actualType);
				hasExitButton = true;
				break;
			case PopupType.Quit:
				isConfirmation = true;
				_popupButton1.SetButton(PopupButtonType.Confirm, actualType);
				_popupButton2.SetButton(PopupButtonType.Cancel, actualType);
				hasExitButton = false;
				break;
			default:
				isConfirmation = false;
				hasExitButton = false;
				break; 


		}

		if(isConfirmation) // needs two button : Is a decision 
		{
			_popupButton1.gameObject.SetActive(true); 
			_popupButton2.gameObject.SetActive(true);
			_popupButton1.SelectButton(); 
		}
		else // needs only one button : Is an information 
		{

			_popupButton1.gameObject.SetActive(true);
			_popupButton2.gameObject.SetActive(false);
			_popupButton1.SelectButton();

		}

		_buttonClose.gameObject.SetActive(hasExitButton);
		if (hasExitButton) // can exit : Has to take the decision or aknowledge the information
		{
			
			_inputReader.closePopupEvent += _closePopupEvent.RaiseEvent;
		
		}
	}

	public void ButtonClicked(int buttonTypeIndex)
	{
		PopupButtonType popupButtonType = (PopupButtonType)buttonTypeIndex;

		switch (popupButtonType)
		{
			case PopupButtonType.Cancel:
				_confirmPopupEvent.RaiseEvent(false);
				break;
			case PopupButtonType.Close:
				_closePopupEvent.RaiseEvent();
				break;
			case PopupButtonType.Confirm:
					_confirmPopupEvent.RaiseEvent(true); 
				break;
			default:
				Debug.Log("Default");
				break; 
				
			
		}

	}
}
