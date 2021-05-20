using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
	[SerializeField] private UIPopupSetter _popupPanel = default;
	[SerializeField] private GameObject _settingsPanel = default;
	[SerializeField] private GameObject _creditsPanel = default;
	[SerializeField] private GameObject _mainMenuPanel = default;

	[SerializeField] private Button _continueButton = default;
	[SerializeField] private Button _NewGameButton = default;

	[SerializeField] private SaveSystem _saveSystem = default;



	[Header("Broadcasting on")]
	[SerializeField]
	private VoidEventChannelSO _startNewGameEvent = default;
	[SerializeField]
	private VoidEventChannelSO _continueGameEvent = default;
	[Header("Listening to")]
	[SerializeField]
	private VoidEventChannelSO _closePopupEvent = default;
	[SerializeField]
	private VoidEventChannelSO _closeSettingsEvent = default;
	[SerializeField]
	private VoidEventChannelSO _closeCreditsEvent = default;

	[SerializeField]
	private BoolEventChannelSO _confirmPopupEvent = default;

	[SerializeField]
	private VoidEventChannelSO _onGameExitEvent = default;


	private bool _hasSaveData;

	[SerializeField] private InputReader _inputReader = default;
	private void Start()
	{
		_inputReader.EnableMenuInput();

		_closePopupEvent.OnEventRaised += HidePopup;

		_closeSettingsEvent.OnEventRaised += CloseSettingsScreen;

		_closeCreditsEvent.OnEventRaised += CloseCreditsScreen;

		SetMenuScreen(); 
	}
	void SetMenuScreen()
	{
		_hasSaveData = _saveSystem.LoadSaveDataFromDisk();
		_continueButton.interactable = _hasSaveData;
		if(_hasSaveData)
		{
	     _continueButton.Select(); 
		}
		else
		{ _NewGameButton.Select();  }


	}

	public void ButtonContinueGameClicked()
	{
		_continueGameEvent.RaiseEvent();

	}
	public void ButtonStartNewGameClicked()
	{
		if(!_hasSaveData)
		{
			ConfirmStartNewGame(); 

		}
		else
		{
			ShowStartNewGameConfirmationPopup(); 

		}
		
	}
	void ConfirmStartNewGame()
	{
		_startNewGameEvent.RaiseEvent();
	}

	void ShowStartNewGameConfirmationPopup()
	{
		_confirmPopupEvent.OnEventRaised += StartNewGamePopupResponse;
		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.NewGame);

	}

	 void StartNewGamePopupResponse(bool startNewGameConfirmed)
	{
		_confirmPopupEvent.OnEventRaised -= StartNewGamePopupResponse;
		_popupPanel.gameObject.SetActive(false);

		if(startNewGameConfirmed)
		{
			ConfirmStartNewGame();

		}
		else
		{
			_continueGameEvent.RaiseEvent(); 
		}

		SetMenuScreen();

	}

	void HidePopup()
	{
	
		_popupPanel.gameObject.SetActive(false);
		SetMenuScreen(); 

	}
	public void OpenSettingsScreen()
	{
		_settingsPanel.SetActive(true);
		_inputReader.closePopupEvent += CloseSettingsScreen;

	}
	public void CloseSettingsScreen()
	{
		_inputReader.closePopupEvent -= CloseSettingsScreen;
		_settingsPanel.SetActive(false);
		SetMenuScreen();

	}
	public void OpenCreditsScreen()
	{
		_creditsPanel.SetActive(true);
		_inputReader.closePopupEvent += CloseCreditsScreen;

		

	}
	public void CloseCreditsScreen()
	{
		_inputReader.closePopupEvent -= CloseCreditsScreen;
		_creditsPanel.SetActive(false);
		SetMenuScreen();


	}


	public void ShowQuitPopup()
	{
		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.Quit);
		_confirmPopupEvent.OnEventRaised += HideQuitPopup; 

	}
	void HideQuitPopup(bool quitConfirmed)
	{
		_confirmPopupEvent.OnEventRaised -= HideQuitPopup;
		_popupPanel.gameObject.SetActive(false); 
		if(quitConfirmed)
		{
			Application.Quit();
			_onGameExitEvent.OnEventRaised(); 
		}
		SetMenuScreen();


	}
	private void OnDestroy()
	{
		_confirmPopupEvent.OnEventRaised -= HideQuitPopup;

		_confirmPopupEvent.OnEventRaised -= StartNewGamePopupResponse;
	}


}
