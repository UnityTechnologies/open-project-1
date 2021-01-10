using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
	[Header("Listening on channels")]
	[Header("Dialogue Events")]
	[FormerlySerializedAs("OpenUIDialogueEvent")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent= default;
	[FormerlySerializedAs("CloseUIDialogueEvent")]
	[SerializeField] private VoidEventChannelSO _closeUIDialogueEvent = default;
	[FormerlySerializedAs("ShowChoicesUIEvent")]
	[SerializeField] private DialogueChoicesChannelSO _showChoicesUIEvent = default;

	[Header("Inventory Events")]
	[FormerlySerializedAs("OpenInventoryScreenEvent")]
	[SerializeField] private VoidEventChannelSO _openInventoryScreenEvent = default;
	[FormerlySerializedAs("OpenInventoryScreenForCookingEvent")]
	[SerializeField] private VoidEventChannelSO _openInventoryScreenForCookingEvent = default;
	[FormerlySerializedAs("CloseInventoryScreenEvent")]
	[SerializeField] private VoidEventChannelSO _closeInventoryScreenEvent = default;

	[Header("Interaction Events")]
	[FormerlySerializedAs("OnInteractionEndedEvent")]
	[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default;
	[FormerlySerializedAs("SetInteractionEvent")]
	[SerializeField] private InteractionUIEventChannelSO _setInteractionEvent = default;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (_openUIDialogueEvent != null)
		{
			_openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
		}
		if (_closeUIDialogueEvent != null)
		{
			_closeUIDialogueEvent.OnEventRaised += CloseUIDialogue;
		}
		if (_openInventoryScreenForCookingEvent != null)
		{
			_openInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;
		}
		if (_openInventoryScreenEvent != null)
		{
			_openInventoryScreenEvent.OnEventRaised += SetInventoryScreen;
		}
		if (_closeInventoryScreenEvent != null)
		{
			_closeInventoryScreenEvent.OnEventRaised += CloseInventoryScreen;
		}
		if (_setInteractionEvent != null)
		{
			_setInteractionEvent.OnEventRaised += SetInteractionPanel;
		}

	}

	private void Start()
	{
		CloseUIDialogue();
	}

	[SerializeField]
	private UIDialogueManager dialogueController = default;

	[SerializeField]
	private UIInventoryManager inventoryPanel = default;

	[SerializeField]
	private UIInteractionManager interactionPanel = default;

	public void OpenUIDialogue(DialogueLineSO dialogueLine, ActorSO actor)
	{
		Debug.Log("Open UI Dialogue");
		dialogueController.SetDialogue(dialogueLine, actor);
		dialogueController.gameObject.SetActive(true);
	}
	public void CloseUIDialogue()
	{
		Debug.Log("Close UI Dialogue");
		dialogueController.gameObject.SetActive(false);
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
		inventoryPanel.gameObject.SetActive(true);

		if (isForCooking)
		{
			inventoryPanel.FillInventory(TabType.recipe, true);

		}
		else
		{
			inventoryPanel.FillInventory();
		}

	}


	public void CloseInventoryScreen()
	{
		inventoryPanel.gameObject.SetActive(false);

		if (isForCooking)
		{
			_onInteractionEndedEvent.RaiseEvent();

		}
	}

	public void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
	{
		if (isOpenEvent)
		{
			interactionPanel.FillInteractionPanel(interactionType);
		}
		interactionPanel.gameObject.SetActive(isOpenEvent);

	}
}
