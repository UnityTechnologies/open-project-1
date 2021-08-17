using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UIPause : MonoBehaviour
{
	[SerializeField] private UIGenericButton _resumeButton = default;
	[SerializeField] private UIGenericButton _settingsButton = default;
	[SerializeField] private UIGenericButton _backToMenuButton = default;

	public UnityAction Resumed = default;
	public UnityAction SettingsScreenOpened = default;
	public UnityAction BackToMainRequested = default;

	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private BoolEventChannelSO _onPauseOpened = default;

	private void OnEnable()
	{
		_onPauseOpened?.RaiseEvent(true);
		_resumeButton.SetButton(true);
		_inputReader.menuCloseEvent += Resume;
		_resumeButton.Clicked += Resume;
		_settingsButton.Clicked += OpenSettingsScreen;
		_backToMenuButton.Clicked += BackToMainMenuConfirmation;
	}



	private void OnDisable()
	{
		_onPauseOpened?.RaiseEvent(false);
		_inputReader.menuCloseEvent -= Resume;
		_resumeButton.Clicked -= Resume;
		_settingsButton.Clicked -= OpenSettingsScreen;
		_backToMenuButton.Clicked -= BackToMainMenuConfirmation;

	}



	void Resume()
	{
		Resumed.Invoke();
	}

	void OpenSettingsScreen()
	{
		SettingsScreenOpened.Invoke();

	}
	void BackToMainMenuConfirmation()
	{
		BackToMainRequested.Invoke();

	}
	public void CloseScreen()
	{
		Resumed.Invoke();
	}

}
