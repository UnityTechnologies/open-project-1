using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private UIDialogueManager _dialogueController = default;

	[SerializeField]
	private UIInventoryManager _inventoryPanel = default;

	[SerializeField]
	private UIInteractionManager _interactionPanel = default;

	[Header("Pause Screen")]
	[SerializeField] private UIPauseScreenSetter _pauseScreen = default;

	[Header("Pause Screen")]
	[SerializeField] private UISettingManager _settingScreen = default;

	[Header("Gameplay Components")]
	[SerializeField]
	private GameStateSO _gameState = default;

	[SerializeField] private InputReader _inputReader = default;
	[Header("Listening on channels")]
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

	[Header("Dialogue Events")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _closeUIDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _closeUIInventoryEvent = default;


	[Header("Inventory Events")]
	[SerializeField] private VoidEventChannelSO _openInventoryScreenForCookingEvent = default;

	[Header("Interaction Events")]
	[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default;
	[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;

	[Header("Pause Events")]
	[SerializeField] private VoidEventChannelSO _clickUnpauseEvent = default;
	[SerializeField] private VoidEventChannelSO _clickBackToMenuEvent = default;
	[Header("Setting Events")]
	[SerializeField] private VoidEventChannelSO _openSettingEvent = default;
	[SerializeField] private VoidEventChannelSO _closeSettingScreenEvent = default;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		_inputReader.openInventoryEvent += SetInventoryScreen;
		_openUIDialogueEvent.OnEventRaised += OpenUIDialogue;

		_closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;

		_openInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;

		_setInteractionEvent.OnEventRaised += SetInteractionPanel;

		_onSceneReady.OnEventRaised += ResetUI;

		_inputReader.pauseEvent += OpenUIPause;
		
		_inputReader.menuUnpauseEvent += CloseUIPause;

		_clickUnpauseEvent.OnEventRaised += CloseUIPause;
		_openSettingEvent.OnEventRaised += OpenSettingScreen;
		_clickBackToMenuEvent.OnEventRaised += BackToMenu;
		_closeSettingScreenEvent.OnEventRaised += CloseSettingScreen; 

	}
	private void OnDestroy()
	{
		//Check if the event exists to avoid errors

		_openUIDialogueEvent.OnEventRaised -= OpenUIDialogue;

		_closeUIDialogueEvent.OnEventRaised -= CloseUIDialogue;

		_closeUIInventoryEvent.OnEventRaised -= CloseInventoryScreen;

		_openInventoryScreenForCookingEvent.OnEventRaised -= SetInventoryScreenForCooking;

		_inputReader.openInventoryEvent -= SetInventoryScreen;

		_setInteractionEvent.OnEventRaised -= SetInteractionPanel;

		_onSceneReady.OnEventRaised -= ResetUI;

		_inputReader.pauseEvent -= OpenUIPause;

		_inputReader.menuUnpauseEvent -= CloseUIPause;

		_closeUIDialogueEvent.OnEventRaised -= CloseUIDialogue;

		_clickUnpauseEvent.OnEventRaised -= CloseUIPause;
		_openSettingEvent.OnEventRaised -= OpenSettingScreen;
		_clickBackToMenuEvent.OnEventRaised -= BackToMenu;
		_closeSettingScreenEvent.OnEventRaised -= CloseSettingScreen;



	}
	void BackToMenu()
	{
		Debug.Log("Back to Menu "); 
	}
	void OpenUIPause()
	{
		_inputReader.pauseEvent -= OpenUIPause;
		_inputReader.pauseEvent += CloseUIPause;
		_inputReader.EnableMenuInput();
		_pauseScreen.gameObject.SetActive(true);
		_pauseScreen.SetPauseScreen(); 
	}

	void CloseUIPause()
	{
		
		_inputReader.pauseEvent += OpenUIPause;
		_inputReader.pauseEvent -= CloseUIPause;
		_pauseScreen.gameObject.SetActive(false);
		_inputReader.EnableGameplayInput();

	}
	void OpenSettingScreen()
	{
		CloseUIPause();
		_inputReader.EnableMenuInput();
		_inputReader.pauseEvent += CloseSettingScreen;
		_settingScreen.gameObject.SetActive(true);

	}

	void CloseSettingScreen()
	{
		_inputReader.EnableGameplayInput();
		_inputReader.pauseEvent -= CloseSettingScreen;
		_settingScreen.gameObject.SetActive(false);

	}
	void ResetUI()
	{
		CloseUIDialogue();
		CloseInventoryScreen();
		SetInteractionPanel(false, InteractionType.None); 
	}

	public void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
	{
		_dialogueController.SetDialogue(dialogueLine, actor);
		_dialogueController.gameObject.SetActive(true);
	}
	public void CloseUIDialogue()
	{
		_dialogueController.gameObject.SetActive(false);
	}

	public void SetInventoryScreenForCooking()
	{
		isForCooking = true;
		OpenInventoryScreen();

	}
	public void SetInventoryScreen()
	{
		isForCooking = false;
		OpenInventoryScreen();

	}
	bool isForCooking = false;
	void OpenInventoryScreen()
	{
		_inventoryPanel.gameObject.SetActive(true);

		_inputReader.EnableMenuInput();
		_inputReader.closeInventoryEvent += CloseInventoryScreen;

		_gameState.UpdateGameState(GameState.Inventory); 

		if (isForCooking)
		{
			_inventoryPanel.FillInventory(TabType.recipe, true);

		}
		else
		{
			_inventoryPanel.FillInventory();
		}

	}


	 void CloseInventoryScreen()
	{
		_inventoryPanel.gameObject.SetActive(false);

		_inputReader.EnableGameplayInput();
		if (isForCooking)
		{
			_onInteractionEndedEvent.RaiseEvent();

		}

		_gameState.ResetToPreviousGameState();

		_inputReader.closeInventoryEvent -= CloseInventoryScreen;
	}

	public void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
	{
		if (isOpenEvent)
		{
			_interactionPanel.FillInteractionPanel(interactionType);
		}
		_interactionPanel.gameObject.SetActive(isOpenEvent);

	}

	
}
