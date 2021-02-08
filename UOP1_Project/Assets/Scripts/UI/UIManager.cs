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

	public VoidEventChannelSO OnInteractionEndedEvent;

	public InteractionUIEventChannelSO SetInteractionEvent;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (OpenUIDialogueEvent != null)
		{
			OpenUIDialogueEvent.RegisterEvent(OpenUIDialogue);
		}
		if (CloseUIDialogueEvent != null)
		{
			CloseUIDialogueEvent.RegisterEvent(CloseUIDialogue);
		}
		if (OpenInventoryScreenForCookingEvent != null)
		{
			OpenInventoryScreenForCookingEvent.RegisterEvent(SetInventoryScreenForCooking);
		}
		if (OpenInventoryScreenEvent != null)
		{
			OpenInventoryScreenEvent.RegisterEvent(SetInventoryScreen);
		}
		if (CloseInventoryScreenEvent != null)
		{
			CloseInventoryScreenEvent.RegisterEvent(CloseInventoryScreen);
		}
		if (SetInteractionEvent != null)
		{
			SetInteractionEvent.RegisterEvent(SetInteractionPanel);
		}

	}

	private void Start()
	{
		CloseUIDialogue();
	}

	[SerializeField]
	UIDialogueManager dialogueController = default;

	[SerializeField]
	UIInventoryManager inventoryPanel;

	[SerializeField]
	UIInteractionManager interactionPanel;

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
			OnInteractionEndedEvent.RaiseEvent();

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
