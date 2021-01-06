using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.
/// Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.
/// </summary>
public class DialogueManager : MonoBehaviour
{ 
//	[SerializeField] private ChoiceBox _choiceBox; // TODO: Demonstration purpose only. Remove or adjust later.

	[SerializeField] private InputReader _inputReader = default;
	public DialogueLineChannelSO OpenUIDialogueEvent;
	public VoidEventChannelSO CloseUIDialogueEvent;
	private DialogueDataSO _currentDialogueDataSO;
	private int _counter;
	private bool _reachedEndOfDialogue { get => _counter >= _currentDialogueDataSO.DialogueLines.Count; }

	/// <summary>
	/// Displays DialogueData in the UI, one by one.
	/// </summary>
	/// <param name="dialogueDataSO"></param>
	public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
	{
		BeginDialogueData(dialogueDataSO);
		DisplayDialogueLine(_currentDialogueDataSO.DialogueLines[_counter], dialogueDataSO.Actor);
	}

	/// <summary>
	/// Prepare DialogueManager when first time displaying DialogueData. 
	/// <param name="dialogueDataSO"></param>
	private void BeginDialogueData(DialogueDataSO dialogueDataSO)
	{
		_counter = 0;
		_inputReader.EnableDialogueInput();
		_inputReader.advanceDialogueEvent += OnAdvance;
		_currentDialogueDataSO = dialogueDataSO;
	}

	/// <summary>
	/// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
	/// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
	/// </summary>
	/// <param name="dialogueLine"></param>
	public void DisplayDialogueLine(DialogueLineSO dialogueLine, ActorSO actor)
	{
if(OpenUIDialogueEvent!=null)
		{

			OpenUIDialogueEvent.RaiseEvent(dialogueLine, actor); 
		}
}

	private void OnAdvance()
	{ 
		_counter++;

		if (!_reachedEndOfDialogue)
		{
			DisplayDialogueLine(_currentDialogueDataSO.DialogueLines[_counter], _currentDialogueDataSO.Actor);
		}
		else
		{
			if(_currentDialogueDataSO.Choices.Count > 0)
			{
				DisplayChoices(_currentDialogueDataSO.Choices);
			}
			else
			{
				DialogueEnded();
			}
		}
	}

	private void DisplayChoices(List<Choice> choices)
	{
		_inputReader.advanceDialogueEvent -= OnAdvance;

		// TODO: Demonstration purpose only. Remove or adjust later.
		//_choiceBox.Show(_currentDialogueDataSO.Choices, this);
	}

	public void DialogueEnded()
	{
		if(CloseUIDialogueEvent!=null)
		CloseUIDialogueEvent.RaiseEvent(); 
		_inputReader.advanceDialogueEvent -= OnAdvance;
		_inputReader.EnableGameplayInput();
	}
}

// TODO: Demonstration purpose only. Remove or adjust later.
/*
[Serializable]
public class ChoiceBox	
{
	[SerializeField] private GameObject _gameObject;
	[SerializeField] private Transform _contentTrans;
	[SerializeField] private GameObject _buttonPrefab;

	public void Show(List<Choice> choices, DialogueManager dialogueManager)
	{
		_gameObject.SetActive(true);

		// Refresh choice box
		foreach(Transform child in _contentTrans)
		{
			GameObject.Destroy(child.gameObject);
		}

		for(int i = 0; i < choices.Count; i++)
		{
			Button choiceButton = GameObject.Instantiate<Button>(_buttonPrefab.GetComponent<Button>(), _contentTrans);
			choiceButton.GetComponentInChildren<TMP_Text>().text = choices[i].OptionName;

			int index = i;
			choiceButton.onClick.AddListener(() => OnChoiceButtonClick(choices[index], dialogueManager)); 
		}
	}

	private void OnChoiceButtonClick(Choice choice, DialogueManager dialogueManager)
	{
		if (choice.Response != null)
			dialogueManager.DisplayDialogueData(choice.Response);
		else
			dialogueManager.DialogueEnded();

		_gameObject.SetActive(false);
	}
}
*/
