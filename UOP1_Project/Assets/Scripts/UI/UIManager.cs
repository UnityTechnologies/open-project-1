using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

	public DialogueLineEvent OpenUIDialogueEvent;
	public VoidGameEvent CloseUIDialogueEvent;

	public VoidGameEvent OpenInventoryScreenEvent;
	public VoidGameEvent CloseInventoryScreenEvent;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (OpenUIDialogueEvent != null)
		{
			OpenUIDialogueEvent.eventRaised += OpenUIDialogue;
		}
		if (CloseUIDialogueEvent != null)
		{
			CloseUIDialogueEvent.eventRaised += CloseUIDialogue;
		}
		if (OpenInventoryScreenEvent != null)
		{
			OpenInventoryScreenEvent.eventRaised += OpenInventoryScreen;
		}
		if (CloseInventoryScreenEvent != null)
		{
			CloseInventoryScreenEvent.eventRaised += CloseInventoryScreen;
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
