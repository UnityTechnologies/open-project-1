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

	[SerializeField] private InputReader _inputReader = default;
	[Header("Listening on channels")]
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

	[Header("Dialogue Events")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _closeUIDialogueEvent = default;

	[Header("Inventory Events")]
	[SerializeField] private VoidEventChannelSO _openInventoryScreenEvent = default;
	[SerializeField] private VoidEventChannelSO _openInventoryScreenForCookingEvent = default;
	[SerializeField] private VoidEventChannelSO _closeInventoryScreenEvent = default;

	[Header("Interaction Events")]
	[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default;
	[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		
			_openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
		
			_closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;
		
			_openInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;
		
			_openInventoryScreenEvent.OnEventRaised += SetInventoryScreen;
		
			_closeInventoryScreenEvent.OnEventRaised += CloseInventoryScreen;
		
			_setInteractionEvent.OnEventRaised += SetInteractionPanel;
		
			_onSceneReady.OnEventRaised += ResetUI;
		

		
	}
	private void OnDestroy()
	{
		//Check if the event exists to avoid errors

		_openUIDialogueEvent.OnEventRaised -= OpenUIDialogue;

		_closeUIDialogueEvent.OnEventRaised -= CloseUIDialogue;

		_openInventoryScreenForCookingEvent.OnEventRaised -= SetInventoryScreenForCooking;

		_openInventoryScreenEvent.OnEventRaised -= SetInventoryScreen;

		_closeInventoryScreenEvent.OnEventRaised -= CloseInventoryScreen;

		_setInteractionEvent.OnEventRaised -= SetInteractionPanel;

		_onSceneReady.OnEventRaised -= ResetUI;



	}
	private void Start()
	{
		
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
		if (isForCooking)
		{
			_inventoryPanel.FillInventory(TabType.recipe, true);

		}
		else
		{
			_inventoryPanel.FillInventory();
		}

	}


	public void CloseInventoryScreen()
	{
		_inventoryPanel.gameObject.SetActive(false);

		_inputReader.EnableGameplayInput();
		if (isForCooking)
		{
			_onInteractionEndedEvent.RaiseEvent();

		}

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
