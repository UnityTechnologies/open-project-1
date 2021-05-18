using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
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
	private bool _reachedEndOfDialogue { get => _counter >= _currentDialogue.DialogueLines.Count; }

	[Header("Listening on channels")]
	[SerializeField] private DialogueDataChannelSO _startDialogue = default;
	[SerializeField] private DialogueChoiceChannelSO _makeDialogueChoiceEvent = default;

	[Header("BoradCasting on channels")]
	[SerializeField] private DialogueLineChannelSO _openUIDialogueEvent = default;
	[SerializeField] private DialogueChoicesChannelSO _showChoicesUIEvent = default;
	[SerializeField] private DialogueDataChannelSO _endDialogue = default;
	[SerializeField] private VoidEventChannelSO _continueWithStep = default;
	[SerializeField] private VoidEventChannelSO _closeDialogueUIEvent = default;

	[Header("Gameplay Components")]
	[SerializeField]
	private GameStateSO _gameState = default;

	private DialogueDataSO _currentDialogue = default;

	private void Start()
	{
			_startDialogue.OnEventRaised += DisplayDialogueData;
		
	}

	/// <summary>
	/// Displays DialogueData in the UI, one by one.
	/// </summary>
	/// <param name="dialogueDataSO"></param>
	public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
	{
		if(_gameState.CurrentGameState != GameState.Cutscene)
		_gameState.UpdateGameState(GameState.Dialogue);
		BeginDialogueData(dialogueDataSO);
		DisplayDialogueLine(_currentDialogue.DialogueLines[_counter], dialogueDataSO.Actor);
	}

	/// <summary>
	/// Prepare DialogueManager when first time displaying DialogueData. 
	/// <param name="dialogueDataSO"></param>
	private void BeginDialogueData(DialogueDataSO dialogueDataSO)
	{
		_counter = 0;
		_inputReader.EnableDialogueInput();
		_inputReader.advanceDialogueEvent += OnAdvance;
		_currentDialogue = dialogueDataSO;
	}

	/// <summary>
	/// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
	/// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
	/// </summary>
	/// <param name="dialogueLine"></param>
	public void DisplayDialogueLine(LocalizedString dialogueLine, ActorSO actor)
	{
		
			_openUIDialogueEvent.RaiseEvent(dialogueLine, actor);
		
	}

	private void OnAdvance()
	{
		_counter++;

		if (!_reachedEndOfDialogue)
		{
			DisplayDialogueLine(_currentDialogue.DialogueLines[_counter], _currentDialogue.Actor);
		}
		else
		{
			if (_currentDialogue.Choices.Count > 0)
			{
				DisplayChoices(_currentDialogue.Choices);
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
		
			_makeDialogueChoiceEvent.OnEventRaised += MakeDialogueChoice;
			_showChoicesUIEvent.RaiseEvent(choices);
		
	}

	private void MakeDialogueChoice(Choice choice)
	{
			_makeDialogueChoiceEvent.OnEventRaised -= MakeDialogueChoice;
		
		if (choice.ActionType == ChoiceActionType.continueWithStep)
		{
			if (_continueWithStep != null)
				_continueWithStep.RaiseEvent();
			if (choice.NextDialogue != null)
				DisplayDialogueData(choice.NextDialogue);
		}
		else
		{
			if (choice.NextDialogue != null)
				DisplayDialogueData(choice.NextDialogue);
			else
				DialogueEndedAndCloseDialogueUI();
		}
	}

	void DialogueEnded()
	{
		if (_endDialogue != null)
			_endDialogue.RaiseEvent(_currentDialogue);

		_gameState.ResetToPreviousGameState();
	}
	public void DialogueEndedAndCloseDialogueUI()
	{
		
		if (_endDialogue != null)
			_endDialogue.RaiseEvent(_currentDialogue);
		if (_closeDialogueUIEvent != null)
			_closeDialogueUIEvent.RaiseEvent();
		_gameState.ResetToPreviousGameState(); 
		_inputReader.advanceDialogueEvent -= OnAdvance;
		_inputReader.EnableGameplayInput();


	}
}

