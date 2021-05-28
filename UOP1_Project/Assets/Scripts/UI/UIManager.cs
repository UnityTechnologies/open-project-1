using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class UIManager : MonoBehaviour
{
	[Header("Scene UI")]
	[SerializeField] private UIPopupSetter _popupPanel = default;

	[SerializeField]private UIDialogueManager _dialogueController = default;

	[SerializeField]private UIInventoryManager _inventoryPanel = default;

	[SerializeField]private UIInteractionManager _interactionPanel = default;

	[SerializeField] private UIPauseScreenSetter _pauseScreen = default;

	[SerializeField] private UISettingManager _settingScreen = default;

	[Header("Gameplay Components")]
	[SerializeField] private GameStateSO _gameState = default;
	[SerializeField] private MenuSO _mainMenu = default;
	[SerializeField] private InputReader _inputReader = default;

	[Header("Listening on channels")]

	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

	[Header("Dialogue Events")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _closeUIDialogueEvent = default;

	[Header("Inventory Events")]
	[SerializeField] private VoidEventChannelSO _openInventoryScreenForCookingEvent = default;
	[SerializeField] private VoidEventChannelSO _closeUIInventoryEvent = default;
	[Header("Interaction Events")]
	[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;
	[Header("Pause Events")]
	[SerializeField] private VoidEventChannelSO _clickUnpauseEvent = default;
	[SerializeField] private VoidEventChannelSO _clickBackToMenuEvent = default;
	[Header("Setting Events")]
	[SerializeField] private VoidEventChannelSO _openSettingEvent = default;
	[SerializeField] private VoidEventChannelSO _closeSettingScreenEvent = default;
	[Header("Popup Events")]
	[SerializeField] private VoidEventChannelSO _closePopupEvent = default;
	[SerializeField] private BoolEventChannelSO _confirmPopupEvent = default;

	[Header("Broadcasting on ")]
	[SerializeField] private LoadEventChannelSO _loadMenuEvent = default;
	[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default;


	bool isForCooking = false;
	
	private void Start()
	{
		_onSceneReady.OnEventRaised += ResetUI;
		_openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
		_closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;
		_inputReader.menuPauseEvent += OpenUIPause; // subscription to open Pause UI event happens in OnEnabled, but the close Event is only subscribed to when the popup is open

		_openInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;
		_setInteractionEvent.OnEventRaised += SetInteractionPanel;
	
		_inputReader.openInventoryEvent += SetInventoryScreen;
		_closeUIInventoryEvent.OnEventRaised += CloseInventoryScreen; 




	}
	void ResetUI()
	{
		_dialogueController.gameObject.SetActive(false);

		_inventoryPanel.gameObject.SetActive(false);

		_pauseScreen.gameObject.SetActive(false);

		_interactionPanel.gameObject.SetActive(false);

		_inputReader.EnableGameplayInput();

		Time.timeScale = 1;

	}
	void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
	{
		_dialogueController.SetDialogue(dialogueLine, actor);
		_dialogueController.gameObject.SetActive(true);
	}
	void CloseUIDialogue()
	{
		_dialogueController.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		//Check if the event exists to avoid errors

		_onSceneReady.OnEventRaised -= ResetUI;
		_openUIDialogueEvent.OnEventRaised -= OpenUIDialogue;
		_closeUIDialogueEvent.OnEventRaised -= CloseUIDialogue;

		_inputReader.menuPauseEvent -= OpenUIPause;

		_openInventoryScreenForCookingEvent.OnEventRaised -= SetInventoryScreenForCooking;
		_setInteractionEvent.OnEventRaised -= SetInteractionPanel;
		_inputReader.openInventoryEvent -= SetInventoryScreen;

		_closeUIInventoryEvent.OnEventRaised -= CloseInventoryScreen;

	}
	void OpenUIPause()
	{
		

		Time.timeScale = 0; // Pause time

		_inputReader.menuPauseEvent -= OpenUIPause; // you cant open the UI Pause again once it's open 
		_inputReader.menuUnpauseEvent += CloseUIPause; // you can close the UI Pause popup if it's open

		_openSettingEvent.OnEventRaised += OpenSettingScreen;//once the UI Pause popup is open, listen to open Settings 
		_clickBackToMenuEvent.OnEventRaised += ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
		_clickUnpauseEvent.OnEventRaised += CloseUIPause;//once the UI Pause popup is open, listen to unpause event


		_pauseScreen.gameObject.SetActive(true);
		_pauseScreen.SetPauseScreen();

		_inputReader.EnableMenuInput();
		_gameState.UpdateGameState(GameState.Pause);
	}

	void CloseUIPause()
	{
		Time.timeScale = 1; // unpause time

		_inputReader.menuPauseEvent += OpenUIPause; // you can open UI pause menu again, if it's closed
		_inputReader.menuUnpauseEvent -= CloseUIPause; // you can't close a closed popup 

		// once the popup is closed, you can't listen to the following events 
		_openSettingEvent.OnEventRaised -= OpenSettingScreen;
		_clickBackToMenuEvent.OnEventRaised -= ShowBackToMenuConfirmationPopup;
		_clickUnpauseEvent.OnEventRaised -= CloseUIPause;

		_pauseScreen.gameObject.SetActive(false);
		_inputReader.EnableGameplayInput();

		_gameState.ResetToPreviousGameState(); 
	}

	void OpenSettingScreen()
	{
		_clickUnpauseEvent.OnEventRaised -= CloseUIPause; // unsub from clause pause popup since it's inactive 

		_inputReader.menuCloseEvent += CloseSettingScreen; // sub to close setting event from input reader 
		_closeSettingScreenEvent.OnEventRaised += CloseSettingScreen; // sub to close setting event with event 

		_pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive
		

		_settingScreen.gameObject.SetActive(true);// set pause screen to active 

		// time is still set to 0 and Input is still set to menuInput 

	}

	void CloseSettingScreen()
	{
		//unsub from close setting events 
		_inputReader.menuCloseEvent -= CloseSettingScreen;
		_closeSettingScreenEvent.OnEventRaised -= CloseSettingScreen;

		_pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive
		_clickUnpauseEvent.OnEventRaised += CloseUIPause; // unsub from clause pause popup since it's inactive

		_settingScreen.gameObject.SetActive(false);

		// time is still set to 0 and Input is still set to menuInput 
		//going out from setting screen gets us back to the pause screen
	}


	void ShowBackToMenuConfirmationPopup()
	{

		_pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive
		_clickUnpauseEvent.OnEventRaised -= CloseUIPause; // unsub from clause pause popup since it's inactive 

		_inputReader.menuCloseEvent += HideBackToMenuConfirmationPopup;
		_closePopupEvent.OnEventRaised += HideBackToMenuConfirmationPopup;

		_confirmPopupEvent.OnEventRaised += BackToMainMenu;

		_inputReader.EnableMenuInput();
		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.BackToMenu);

	}

	void BackToMainMenu(bool confirm)
	{
		_confirmPopupEvent.OnEventRaised -= BackToMainMenu;

		HideBackToMenuConfirmationPopup();// hide confirmation screen, show close UI pause, 


		if (confirm) 
			
		{
			CloseUIPause();//close ui pause to unsub from all events 
			_loadMenuEvent.RaiseEvent(_mainMenu, false); //load main menu
		}
	
	}
	void HideBackToMenuConfirmationPopup()
	{
		_inputReader.menuCloseEvent -= HideBackToMenuConfirmationPopup;
		_closePopupEvent.OnEventRaised -= HideBackToMenuConfirmationPopup;

		_pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive
		_clickUnpauseEvent.OnEventRaised += CloseUIPause; // unsub from clause pause popup since it's inactive 

		_popupPanel.gameObject.SetActive(false);

		// time is still set to 0 and Input is still set to menuInput 
		//going out from confirmaiton popup screen gets us back to the pause screen


	}
	void SetInventoryScreenForCooking()
	{
		isForCooking = true;
		OpenInventoryScreen();

	}
	 void SetInventoryScreen()
	{
		isForCooking = false;
		OpenInventoryScreen();

	}
	void OpenInventoryScreen()
	{
		_inputReader.menuPauseEvent -= OpenUIPause; // you cant open the UI Pause again when you are in inventory  
		_inputReader.menuUnpauseEvent -= CloseUIPause; // you can close the UI Pause popup when you are in inventory 

		_inputReader.menuCloseEvent += CloseInventoryScreen;
	

		if (isForCooking)
		{
			_inventoryPanel.FillInventory(TabType.recipe, true);

		}
		else
		{
			_inventoryPanel.FillInventory();
		}
	
		_inventoryPanel.gameObject.SetActive(true);

		_inputReader.EnableMenuInput();

		_gameState.UpdateGameState(GameState.Inventory);
	}
	void CloseInventoryScreen()
	{

		_inputReader.menuPauseEvent += OpenUIPause; // you cant open the UI Pause again when you are in inventory  
	
		_inputReader.menuCloseEvent -= CloseInventoryScreen;
		

		_inventoryPanel.gameObject.SetActive(false);
	
		if (isForCooking)
		{
			_onInteractionEndedEvent.RaiseEvent();

		}

		_inputReader.EnableGameplayInput();
		_gameState.ResetToPreviousGameState();
	}


	 void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
	{
		if (isOpenEvent)
		{
			_interactionPanel.FillInteractionPanel(interactionType);
		}
		_interactionPanel.gameObject.SetActive(isOpenEvent);

	}

	
}
