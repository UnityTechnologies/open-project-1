using UnityEngine;
using UnityEngine.Events;

public class UIPause : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private UIGenericButton _resumeButton = default;
	[SerializeField] private UIGenericButton _settingsButton = default;
	[SerializeField] private UIGenericButton _backToMenuButton = default;

	[Header("Listening to")]
	[SerializeField] private BoolEventChannelSO _onPauseOpened = default;

	public event UnityAction Resumed = default;
	public event UnityAction SettingsScreenOpened = default;
	public event UnityAction BackToMainRequested = default;

	private void OnEnable()
	{
		_onPauseOpened.RaiseEvent(true);

		_resumeButton.SetButton(true);
		_inputReader.MenuCloseEvent += Resume;
		_resumeButton.Clicked += Resume;
		_settingsButton.Clicked += OpenSettingsScreen;
		_backToMenuButton.Clicked += BackToMainMenuConfirmation;
	}

	private void OnDisable()
	{
		_onPauseOpened.RaiseEvent(false);
		
		_inputReader.MenuCloseEvent -= Resume;
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
