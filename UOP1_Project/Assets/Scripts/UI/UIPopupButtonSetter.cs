using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables; 

public class UIPopupButtonSetter : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _buttonText = default;
	[SerializeField] private Button _button = default;

	PopupButtonType _currentType = default;

	[SerializeField]
	private IntEventChannelSO _buttonClickedEvent = default;

	public void SetButton(PopupButtonType _type, PopupType popupType)
	{
		_currentType = _type;
		_buttonText.StringReference.TableEntryReference = _currentType.ToString() + "_"+ popupType.ToString();
	}
	public void SelectButton()
	{
		_button.Select();
	}

	public void ButtonClicked()
	{
		int idType = (int)_currentType;
		Debug.Log(idType); 
		_buttonClickedEvent.RaiseEvent(idType); 
	}
}
