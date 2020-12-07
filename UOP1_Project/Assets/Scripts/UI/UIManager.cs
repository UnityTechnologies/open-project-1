using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

	public DialogueLineChannelSO OpenUIDialogueEvent;
	public VoidEventChannelSO CloseUIDialogueEvent;

	public VoidEventChannelSO OpenInventoryScreenEvent;
	public VoidEventChannelSO OpenInventoryScreenForCookingEvent;
	public VoidEventChannelSO CloseInventoryScreenEvent;

	public InteractionUIEventChannelSO SetInteractionEvent;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (OpenUIDialogueEvent != null)
		{
			OpenUIDialogueEvent.OnEventRaised += OpenUIDialogue;
		}
		if (CloseUIDialogueEvent != null)
		{
			CloseUIDialogueEvent.OnEventRaised += CloseUIDialogue;
		}
		if (OpenInventoryScreenForCookingEvent != null)
		{
			OpenInventoryScreenForCookingEvent.OnEventRaised += SetInventoryScreenForCooking;
		}
		if (OpenInventoryScreenEvent != null)
		{
			OpenInventoryScreenEvent.OnEventRaised += SetInventoryScreen;
		}
		if (CloseInventoryScreenEvent != null)
		{
			CloseInventoryScreenEvent.OnEventRaised += CloseInventoryScreen;
		}
		if (SetInteractionEvent != null)
		{
			SetInteractionEvent.OnEventRaised += SetInteractionPanel;
		}
	}

	private void Start()
	{
		CloseUIDialogue();
	}

	[SerializeField]
	DialogueUIController dialogueController = default;

	[SerializeField]
	InventoryFiller inventoryPanel;

	[SerializeField]
	UIInteractionPanelFiller interactionPanel;

	public void OpenUIDialogue(DialogueLineSO dialogueLine)
	{
		dialogueController.SetDialogue(dialogueLine);
		dialogueController.gameObject.SetActive(true);
	}
	public void CloseUIDialogue()
	{
		dialogueController.gameObject.SetActive(false);
	}

	public void SetInventoryScreenForCooking()
	{

		OpenInventoryScreen(true);

	}
	public void SetInventoryScreen()
	{

		OpenInventoryScreen(false);

	}

	void OpenInventoryScreen(bool isForCooking)
	{
		inventoryPanel.gameObject.SetActive(true);

		if (isForCooking)
		{
			inventoryPanel.FillInventory(TabType.recipe);

		}
		else
		{
			inventoryPanel.FillInventory();
		}

	}


	public void CloseInventoryScreen()
	{
		inventoryPanel.gameObject.SetActive(false);

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
