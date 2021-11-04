using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class UIManager : MonoBehaviour
{
	[Header("Scene UI")]
	[SerializeField] private MenuSelectionHandler _selectionHandler = default;
	[SerializeField] private UIPopup _popupPanel = default;
	[SerializeField] private UIDialogueManager _dialogueController = default;
	[SerializeField] private UIInventory _inventoryPanel = default;
	[SerializeField] private UIInteraction _interactionPanel = default;
	[SerializeField] private GameObject _switchTabDisplay = default;
	[SerializeField] private UIItemForAnimation _cookingAnimation = default;
	[SerializeField] private UIPause _pauseScreen = default;
	[SerializeField] private UISettingsController _settingScreen = default;

	[Header("Gameplay")]
	[SerializeField] private GameStateSO _gameStateManager = default;
	[SerializeField] private MenuSO _mainMenu = default;
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private ActorSO _mainProtagonist = default;

	[Header("Listening on")]
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

	[Header("Dialogue Events")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private IntEventChannelSO _closeUIDialogueEvent = default;

	[Header("Inventory Events")]
	[SerializeField] private VoidEventChannelSO _openInventoryScreenForCookingEvent = default;
	[SerializeField] private ItemEventChannelSO _cookRecipeEvent = default;

	[Header("Interaction Events")]
	[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;

	[Header("Broadcasting on ")]
	[SerializeField] private LoadEventChannelSO _loadMenuEvent = default;
	[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default;

	bool isForCooking = false;

	private void OnEnable()
	{
		_onSceneReady.OnEventRaised += ResetUI;
		_openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
		_closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;
		_inputReader.MenuPauseEvent += OpenUIPause; // subscription to open Pause UI event happens in OnEnabled, but the close Event is only subscribed to when the popup is open
		_openInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;
		_setInteractionEvent.OnEventRaised += SetInteractionPanel;
		_inputReader.OpenInventoryEvent += SetInventoryScreen;
		_inventoryPanel.Closed += CloseInventoryScreen;
		_cookRecipeEvent.OnEventRaised += PlayCookingAnimation;
	}

	private void OnDisable()
	{
		_onSceneReady.OnEventRaised -= ResetUI;
		_openUIDialogueEvent.OnEventRaised -= OpenUIDialogue;
		_closeUIDialogueEvent.OnEventRaised -= CloseUIDialogue;
		_inputReader.MenuPauseEvent -= OpenUIPause;
		_openInventoryScreenForCookingEvent.OnEventRaised -= SetInventoryScreenForCooking;
		_setInteractionEvent.OnEventRaised -= SetInteractionPanel;
		_inputReader.OpenInventoryEvent -= SetInventoryScreen;
		_inventoryPanel.Closed -= CloseInventoryScreen;
		_cookRecipeEvent.OnEventRaised -= PlayCookingAnimation;
	}

	void ResetUI()
	{
		_dialogueController.gameObject.SetActive(false);
		_inventoryPanel.gameObject.SetActive(false);
		_pauseScreen.gameObject.SetActive(false);
		_interactionPanel.gameObject.SetActive(false);
		_switchTabDisplay.SetActive(false);
		_cookingAnimation.gameObject.SetActive(false);

		Time.timeScale = 1;
	}

	void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
	{
		bool isProtagonistTalking = (actor == _mainProtagonist);
		_dialogueController.SetDialogue(dialogueLine, actor, isProtagonistTalking);
		_interactionPanel.gameObject.SetActive(false);
		_dialogueController.gameObject.SetActive(true);
	}

	void CloseUIDialogue(int dialogueType)
	{
		_selectionHandler.Unselect();
		_dialogueController.gameObject.SetActive(false);
		_onInteractionEndedEvent.RaiseEvent();
	}

	void OpenUIPause()
	{
		_inputReader.MenuPauseEvent -= OpenUIPause; // you can open UI pause menu again, if it's closed

		Time.timeScale = 0; // Pause time

		_pauseScreen.SettingsScreenOpened += OpenSettingScreen;//once the UI Pause popup is open, listen to open Settings 
		_pauseScreen.BackToMainRequested += ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
		_pauseScreen.Resumed += CloseUIPause;//once the UI Pause popup is open, listen to unpause event

		_pauseScreen.gameObject.SetActive(true);

		_inputReader.EnableMenuInput();
		_gameStateManager.UpdateGameState(GameState.Pause);
	}

	void CloseUIPause()
	{
		Time.timeScale = 1; // unpause time

		_inputReader.MenuPauseEvent += OpenUIPause; // you can open UI pause menu again, if it's closed

		// once the popup is closed, you can't listen to the following events 
		_pauseScreen.SettingsScreenOpened -= OpenSettingScreen;//once the UI Pause popup is open, listen to open Settings 
		_pauseScreen.BackToMainRequested -= ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
		_pauseScreen.Resumed -= CloseUIPause;//once the UI Pause popup is open, listen to unpause event

		_pauseScreen.gameObject.SetActive(false);

		_gameStateManager.ResetToPreviousGameState();
		
		if (_gameStateManager.CurrentGameState == GameState.Gameplay
			|| _gameStateManager.CurrentGameState == GameState.Combat)
			{
				_inputReader.EnableGameplayInput();
			}

		_selectionHandler.Unselect();
	}

	void OpenSettingScreen()
	{
		_settingScreen.Closed += CloseSettingScreen; // sub to close setting event with event 

		_pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive

		_settingScreen.gameObject.SetActive(true);// set Setting screen to active 

		// time is still set to 0 and Input is still set to menuInput 
	}

	void CloseSettingScreen()
	{
		//unsub from close setting events 
		_settingScreen.Closed -= CloseSettingScreen;

		_selectionHandler.Unselect();
		_pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive

		_settingScreen.gameObject.SetActive(false);

		// time is still set to 0 and Input is still set to menuInput 
		//going out from setting screen gets us back to the pause screen
	}

	void ShowBackToMenuConfirmationPopup()
	{
		_pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive

		_popupPanel.ClosePopupAction += HideBackToMenuConfirmationPopup;

		_popupPanel.ConfirmationResponseAction += BackToMainMenu;

		_inputReader.EnableMenuInput();
		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.BackToMenu);
	}

	void BackToMainMenu(bool confirm)
	{
		HideBackToMenuConfirmationPopup();// hide confirmation screen, show close UI pause, 

		if (confirm)
		{
			CloseUIPause();//close ui pause to unsub from all events 
			_loadMenuEvent.RaiseEvent(_mainMenu, false); //load main menu
		}
	}
	
	void HideBackToMenuConfirmationPopup()
	{
		_popupPanel.ClosePopupAction -= HideBackToMenuConfirmationPopup;
		_popupPanel.ConfirmationResponseAction -= BackToMainMenu;

		_popupPanel.gameObject.SetActive(false);
		_selectionHandler.Unselect();
		_pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive

		// time is still set to 0 and Input is still set to menuInput 
		//going out from confirmaiton popup screen gets us back to the pause screen
	}

	void SetInventoryScreenForCooking()
	{
		if (_gameStateManager.CurrentGameState == GameState.Gameplay)
		{
			isForCooking = true;
			_interactionPanel.gameObject.SetActive(false);
			OpenInventoryScreen();
		}
	}

	void SetInventoryScreen()
	{
		if (_gameStateManager.CurrentGameState == GameState.Gameplay)
		{
			isForCooking = false;
			OpenInventoryScreen();
		}
	}

	void OpenInventoryScreen()
	{
		_inputReader.MenuPauseEvent -= OpenUIPause; // player cant open the UI Pause again when they are in inventory  
		_inputReader.MenuUnpauseEvent -= CloseUIPause; // player can close the UI Pause popup when they are in inventory 

		_inputReader.MenuCloseEvent += CloseInventoryScreen;
		_inputReader.CloseInventoryEvent += CloseInventoryScreen;
		if (isForCooking)
		{
			_inventoryPanel.FillInventory(InventoryTabType.Recipe, true);

		}
		else
		{
			_inventoryPanel.FillInventory();
		}

		_inventoryPanel.gameObject.SetActive(true);
		_switchTabDisplay.SetActive(true);
		_inputReader.EnableMenuInput();

		_gameStateManager.UpdateGameState(GameState.Inventory);
	}

	void CloseInventoryScreen()
	{
		_inputReader.MenuPauseEvent += OpenUIPause; // you cant open the UI Pause again when you are in inventory  

		_inputReader.MenuCloseEvent -= CloseInventoryScreen;
		_inputReader.CloseInventoryEvent -= CloseInventoryScreen;

		_switchTabDisplay.SetActive(false);
		_inventoryPanel.gameObject.SetActive(false);

		if (isForCooking)
		{
			_onInteractionEndedEvent.RaiseEvent();
		}
		_selectionHandler.Unselect();
		_gameStateManager.ResetToPreviousGameState();
		if (_gameStateManager.CurrentGameState == GameState.Gameplay || _gameStateManager.CurrentGameState == GameState.Combat)
			_inputReader.EnableGameplayInput();
	}

	void SetInteractionPanel(bool isOpen, InteractionType interactionType)
	{
		if (_gameStateManager.CurrentGameState != GameState.Combat)
		{
			if (isOpen)
			{
				_interactionPanel.FillInteractionPanel(interactionType);
			}

			_interactionPanel.gameObject.SetActive(isOpen);
		}
		else if (!isOpen)
		{
			_interactionPanel.gameObject.SetActive(isOpen);
		}
	}

	public void PlayCookingAnimation(ItemSO itemToCook)
	{
		CloseInventoryScreen();
		_cookingAnimation.SetItem(itemToCook);
		_cookingAnimation.gameObject.SetActive(true);
		_cookingAnimation.AnimationEnded += StopCookingAnimation;
	}

	public void StopCookingAnimation()
	{
		_cookingAnimation.AnimationEnded -= StopCookingAnimation;
		_cookingAnimation.gameObject.SetActive(false);
	}
}