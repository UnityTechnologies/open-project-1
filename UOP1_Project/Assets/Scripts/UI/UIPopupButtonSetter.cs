using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables; 

public class UIPopupButtonSetter : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _buttonText = default;
	[SerializeField] private MultiInputButton _button = default;

	PopupButtonType _currentType = default;

	[SerializeField]
	private IntEventChannelSO _buttonClickedEvent = default;

	public void SetButton(PopupButtonType _type, PopupType popupType, bool isSelected)
	{
		_currentType = _type;
		_buttonText.StringReference.TableEntryReference = _currentType.ToString() + "_"+ popupType.ToString();

		if(isSelected)
		  SelectButton(); 

	}

    void SelectButton()
	{
		_button.Select(); 
		_button.UpdateSelected();
	}

	public void ButtonClicked()
	{
		int idType = (int)_currentType;
		_buttonClickedEvent.RaiseEvent(idType); 
	}
}
