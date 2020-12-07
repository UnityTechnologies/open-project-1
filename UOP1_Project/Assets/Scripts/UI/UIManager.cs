using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

	public DialogueLineChannelSO OpenUIDialogueEvent;
	public VoidEventChannelSO CloseUIDialogueEvent;

	public VoidEventChannelSO OpenInventoryScreenEvent;
	public VoidEventChannelSO CloseInventoryScreenEvent;

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
		if (OpenInventoryScreenEvent != null)
		{
			OpenInventoryScreenEvent.OnEventRaised += OpenInventoryScreen;
		}
		if (CloseInventoryScreenEvent != null)
		{
			CloseInventoryScreenEvent.OnEventRaised += CloseInventoryScreen;
		}
	}

	private void Start()
	{
		CloseUIDialogue();
	}

	[SerializeField] DialogueUIController dialogueController = default;

	[SerializeField]
	InventoryFiller inventoryPanel;

	public void OpenUIDialogue(DialogueLineSO dialogueLine)
	{
		dialogueController.SetDialogue(dialogueLine);
		dialogueController.gameObject.SetActive(true);
	}
	public void CloseUIDialogue()
	{
		dialogueController.gameObject.SetActive(false);
	}



	public void OpenInventoryScreen()
	{
		inventoryPanel.gameObject.SetActive(true);
		inventoryPanel.FillInventory();


	}

	public void CloseInventoryScreen()
	{
		inventoryPanel.gameObject.SetActive(false);

	}
}
