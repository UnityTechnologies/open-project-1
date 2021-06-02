using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Events;

public enum PopupButtonType
{
	Confirm,
	Cancel,
	Close,
	DoNothing,
}
public enum PopupType
{
	Quit,
	NewGame,
	BackToMenu,
}
public class UIPopup : MonoBehaviour
{


	[SerializeField] private LocalizeStringEvent _titleText = default;
	[SerializeField] private LocalizeStringEvent _descriptionText = default;
	[SerializeField]
	private Button _buttonClose = default;
	[SerializeField] private UIButtonSetter _popupButton1 = default;
	[SerializeField] private UIButtonSetter _popupButton2 = default;

	[SerializeField] private InputReader _inputReader = default;

	PopupType actualType;


	public UnityAction<bool> ConfirmationResponseAction;
	public UnityAction ClosePopupAction;

	private void OnDisable()
	{
		_popupButton2.Clicked -= CancelButtonClicked;
		_popupButton1.Clicked -= ConfirmButtonClicked;
		_inputReader.menuCloseEvent -= ClosePopupButtonClicked;
	}
	public void SetPopup(PopupType popupType)
	{
		actualType = popupType;
		bool isConfirmation = false;
		bool hasExitButton = false;
		_titleText.StringReference.TableEntryReference = actualType.ToString() + "_Popup_Title";
		_descriptionText.StringReference.TableEntryReference = actualType.ToString() + "_Popup_Description";
		string tableEntryReferenceConfirm = PopupButtonType.Confirm + "_" + actualType;
		string tableEntryReferenceCancel = PopupButtonType.Cancel + "_" + actualType;
		switch (actualType)
		{
			case PopupType.NewGame:
			case PopupType.BackToMenu:
				isConfirmation = true;

				_popupButton1.SetButton(tableEntryReferenceConfirm, true);
				_popupButton2.SetButton(tableEntryReferenceCancel, false);
				hasExitButton = true;
				break;
			case PopupType.Quit:
				isConfirmation = true;
				_popupButton1.SetButton(tableEntryReferenceConfirm, true);
				_popupButton2.SetButton(tableEntryReferenceCancel, false);
				hasExitButton = false;
				break;
			default:
				isConfirmation = false;
				hasExitButton = false;
				break;


		}

		if (isConfirmation) // needs two button : Is a decision 
		{
			_popupButton1.gameObject.SetActive(true);
			_popupButton2.gameObject.SetActive(true);

			_popupButton2.Clicked += CancelButtonClicked;
			_popupButton1.Clicked += ConfirmButtonClicked;
		}
		else // needs only one button : Is an information 
		{


			_popupButton1.gameObject.SetActive(true);
			_popupButton2.gameObject.SetActive(false);

			_popupButton1.Clicked += ConfirmButtonClicked;

		}

		_buttonClose.gameObject.SetActive(hasExitButton);

		if (hasExitButton) // can exit : Has to take the decision or aknowledge the information
		{

			_inputReader.menuCloseEvent += ClosePopupButtonClicked;

		}
	}




	public void ClosePopupButtonClicked()
	{
		ClosePopupAction.Invoke();

	}
	void ConfirmButtonClicked()
	{
		ConfirmationResponseAction.Invoke(true);
	}
	void CancelButtonClicked()
	{
		ConfirmationResponseAction.Invoke(false);


	}

}
