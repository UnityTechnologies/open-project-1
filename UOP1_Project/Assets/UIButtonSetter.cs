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
		_button.onClick.RemoveAllListeners();
		_button.onClick.AddListener(()=>{ Debug.Log("Button");  _buttonClickedEvent.RaiseEvent();});
		if(select)
		_button.Select(); 
	}

}
