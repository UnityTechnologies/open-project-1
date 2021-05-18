using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPauseScreenSetter : MonoBehaviour
{
	[SerializeField] private UIButtonSetter _unpauseButton = default;
	[SerializeField] private UIButtonSetter _settingsButton = default;
	[SerializeField] private UIButtonSetter _backToMenuButton = default;
	[SerializeField] private Button _closeButton = default;
	[SerializeField] private VoidEventChannelSO _clickUnpauseEvent = default;
	[SerializeField] private VoidEventChannelSO _clickSettingsEvent = default;
	[SerializeField] private VoidEventChannelSO _clickBackToMenuEvent = default;

	[SerializeField] private InputReader _inputReader = default;


	public void SetPauseScreen()
	{
		_inputReader.closePopupEvent += _clickUnpauseEvent.RaiseEvent;

		_unpauseButton.SetButton(_clickUnpauseEvent, true);

		_settingsButton.SetButton(_clickSettingsEvent, false);

		_backToMenuButton.SetButton(_clickBackToMenuEvent, false);
	
	}
    

}
