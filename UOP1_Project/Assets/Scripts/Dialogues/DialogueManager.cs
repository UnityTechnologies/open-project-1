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
	[SerializeField] private IntEventChannelSO _endDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _continueWithStep = default;
	[SerializeField] private VoidEventChannelSO _playWinningQuest = default;
	[SerializeField] private VoidEventChannelSO _playLosingQuest = default;

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
		if (_gameState.CurrentGameState != GameState.Cutscene)
			_gameState.UpdateGameState(GameState.Dialogue);
		_counter = 0;
		_inputReader.EnableDialogueInput();
		_inputReader.advanceDialogueEvent += OnAdvance;
		_currentDialogue = dialogueDataSO;


		if (_currentDialogue.DialogueLines != null)
			DisplayDialogueLine(_currentDialogue.DialogueLines[_counter], dialogueDataSO.Actor);
		else
		{
			Debug.LogError("Check Dialogue");
		}
	}
	//TODO : Check if there's no dependencies, and remove this function if none 
	/// <summary>
	/// Prepare DialogueManager when first time displaying DialogueData. 
	/// <param name="dialogueDataSO"></param>
	private void BeginDialogue(DialogueDataSO dialogueDataSO)
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

		switch (choice.ActionType)
		{
			case ChoiceActionType.continueWithStep:
				if (_continueWithStep != null)
					_continueWithStep.RaiseEvent();
				if (choice.NextDialogue != null)
					DisplayDialogueData(choice.NextDialogue);
				break;
			case ChoiceActionType.winningChoice:
				if (_playWinningQuest != null)
					_playWinningQuest.RaiseEvent();
				if (choice.NextDialogue != null)
					DisplayDialogueData(choice.NextDialogue);
				else
					DialogueEndedAndCloseDialogueUI();
				break;
			case ChoiceActionType.losingChoice:
				if (_playLosingQuest != null)
					_playLosingQuest.RaiseEvent();
				if (choice.NextDialogue != null)
					DisplayDialogueData(choice.NextDialogue);
				else
					DialogueEndedAndCloseDialogueUI();

				break;
			case ChoiceActionType.doNothing:
				if (choice.NextDialogue != null)
					DisplayDialogueData(choice.NextDialogue);
				else
					DialogueEndedAndCloseDialogueUI();
				break;

		}



	}

	public void CutsceneDialogueEnded()
	{

		if (_endDialogueEvent != null)
			_endDialogueEvent.RaiseEvent((int)DialogueType.defaultDialogue);
	}
	void DialogueEndedAndCloseDialogueUI()
	{
		//raise the special event for end of dialogue if any 
		_currentDialogue.FinishDialogue();
		//raise end dialogue event 
		if (_endDialogueEvent != null)
			_endDialogueEvent.RaiseEvent((int)_currentDialogue.DialogueType);
		_gameState.ResetToPreviousGameState();
		_inputReader.advanceDialogueEvent -= OnAdvance;
		_inputReader.EnableGameplayInput();



	}
}

