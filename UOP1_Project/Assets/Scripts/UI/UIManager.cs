using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	//TODO: Remove Singleton
	public static UIManager Instance;

	private void Start()
	{
		Instance = this;
		CloseUIDialogue();
	}

	[SerializeField] DialogueUIController dialogueController = default;

	public void OpenUIDialogue(DialogueLineSO dialogueLine)
	{
		dialogueController.SetDialogue(dialogueLine);
		dialogueController.gameObject.SetActive(true);
	}
	public void CloseUIDialogue()
	{
		dialogueController.gameObject.SetActive(false);
	}
}
