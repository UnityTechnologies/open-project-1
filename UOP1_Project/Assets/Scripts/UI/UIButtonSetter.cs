using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIButtonSetter : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _buttonText = default;
	[SerializeField] private Button _button = default;
	 private VoidEventChannelSO _buttonClickedEvent = default;

	public void SetButton(VoidEventChannelSO buttonEvent, bool select)
	{
		_buttonClickedEvent = buttonEvent;
		if(select)
		_button.Select(); 
	}
	public void Click()
	{
		_buttonClickedEvent.RaiseEvent();

	}

}
