using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.
/// Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.
/// </summary>
public class DialogueManager : MonoBehaviour
{ 
//	[SerializeField] private ChoiceBox _choiceBox; // TODO: Demonstration purpose only. Remove or adjust later.

	[SerializeField] private InputReader _inputReader = default;
	private int _counter;
	private bool _reachedEndOfDialogue { get => _counter >= _currentDialogueDataSO.DialogueLines.Count; }

	[Header("Listening on channels")]
	[SerializeField] private DialogueDataChannelSO _startDialogue = default;
	[SerializeField] private DialogueChoiceChannelSO _makeDialogueChoiceEvent = default;

	[Header("BoradCasting on channels")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private DialogueChoicesChannelSO _showChoicesUIEvent = default;
	[SerializeField] private VoidEventChannelSO _endDialogue = default;
	[SerializeField] private VoidEventChannelSO _closeDialogueUIEvent = default;


	private DialogueDataSO _currentDialogueDataSO = default;

	private void Start()
	{
		if(_startDialogue!=null)
		{
			_startDialogue.OnEventRaised += DisplayDialogueData; 
		}
		
	}

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
		Debug.Log("DisplayDialogueLine"); 
        if(_openUIDialogueEvent!=null)
		{
			_openUIDialogueEvent.RaiseEvent(dialogueLine, actor); 
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
				DialogueEndedAndCloseDialogueUI();
			}
		}
	}

	private void DisplayChoices(List<Choice> choices)
	{
		_inputReader.advanceDialogueEvent -= OnAdvance;

		if (_makeDialogueChoiceEvent != null)
		{
			_makeDialogueChoiceEvent.OnEventRaised += MakeDialogueChoice;
		}

		if (_showChoicesUIEvent != null)
		{
			_showChoicesUIEvent.RaiseEvent(choices);
		}
	}

	private void MakeDialogueChoice(Choice choice)
	{
		if (_makeDialogueChoiceEvent != null)
		{
			_makeDialogueChoiceEvent.OnEventRaised -= MakeDialogueChoice;
		}
		if (choice.NextDialogue != null)
			DisplayDialogueData(choice.NextDialogue);
		else
			DialogueEnded();
	}

	 void DialogueEnded()
	{
		Debug.Log("DialogueEnded");
		if (_endDialogue!=null)
		    _endDialogue.RaiseEvent();
	}
	public void DialogueEndedAndCloseDialogueUI()
	{
		Debug.Log("DialogueEnded");
		if (_endDialogue != null)
			_endDialogue.RaiseEvent();
		if (_closeDialogueUIEvent != null)
			_closeDialogueUIEvent.RaiseEvent();
		_inputReader.advanceDialogueEvent -= OnAdvance;
		_inputReader.EnableGameplayInput();


	}
}

