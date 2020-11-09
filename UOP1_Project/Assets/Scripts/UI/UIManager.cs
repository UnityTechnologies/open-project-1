using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

	public static UIManager Instance;

	private void Start()
	{
		Instance = this;
		CloseUIDialogue(); 
	}

	[SerializeField]
	DialogueUIController dialogueController;

	public void OpenUIDialogue(DialogueLineSO dialogueLine) {

		dialogueController.gameObject.SetActive(true);
		dialogueController.SetDialogue( dialogueLine); 

	}
	public void CloseUIDialogue()
	{
		dialogueController.gameObject.SetActive(false);
	}
}
