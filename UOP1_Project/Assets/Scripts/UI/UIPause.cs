using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UIPause : MonoBehaviour
{
	[SerializeField] private UIButtonSetter _resumeButton = default;
	[SerializeField] private UIButtonSetter _settingsButton = default;
	[SerializeField] private UIButtonSetter _backToMenuButton = default;

	public UnityAction Resumed = default;
	public UnityAction SettingsScreenOpened = default;
	public UnityAction BackToMainRequested = default;

	[SerializeField] private InputReader _inputReader = default;

	private void OnEnable()
	{
		_resumeButton.SetButton(true);
		_inputReader.menuCloseEvent += Resume;
		_resumeButton.Clicked += Resume;
		_settingsButton.Clicked += OpenSettingsScreen;
		_backToMenuButton.Clicked += BackToMainMenuConfirmation;
	}



	private void OnDisable()
	{
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
