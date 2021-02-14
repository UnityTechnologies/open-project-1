using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script needs to be put on the actor, and takes care of the current step to accomplish.
//the step contains a dialogue and maybe an event.

public class StepController : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private ActorSO _actor = default;
	[SerializeField] private DialogueDataSO _defaultDialogue = default;
	[SerializeField] private QuestAncorSO _questAnchor = default;

	[Header("Listening to channels")]
	//[SerializeField] private StepChannelSO _startStepEvent = default;
	//[SerializeField] private DialogueDataChannelSO _endDialogueEvent = default;
	[SerializeField] private DialogueActorChannelSO _interactionEvent = default;
	//[SerializeField] private DialogueActorChannelSO _PlayDefaultEvent = default;
	[SerializeField] private VoidEventChannelSO _winDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _loseDialogueEvent = default;
	//[SerializeField] private VoidEventChannelSO _continueWithStep = default;
	//[SerializeField] private VoidEventChannelSO _endStepEvent = default;

	[Header("Broadcasting on channels")]
	//[SerializeField] private VoidEventChannelSO _checkStepValidityEvent = default;
	[SerializeField] private DialogueDataChannelSO _startDialogueEvent = default;

	//check if character is actif. An actif character is the character concerned by the step.

	private DialogueDataSO _currentDialogue;

	private void Start()
	{

		if (_winDialogueEvent != null)
		{ _winDialogueEvent.OnEventRaised += PlayWinDialogue; }
		if (_loseDialogueEvent != null)
		{ _loseDialogueEvent.OnEventRaised += PlayLoseDialogue; }


	}

	void PlayDefaultDialogue()
	{

		if (_defaultDialogue != null)
		{
			_currentDialogue = _defaultDialogue;
			StartDialogue();
		}

	}


	//start a dialogue when interaction
	//some Steps need to be instantanious. And do not need the interact button.
	//when interaction again, restart same dialogue.
	public void InteractWithCharacter()
	{

		DialogueDataSO displayDialogue = _questAnchor.InteractWithCharacter(_actor, false, false);
		Debug.Log("dialogue " + displayDialogue + "actor" + _actor);
		if (displayDialogue != null)
		{
			_currentDialogue = displayDialogue;
			StartDialogue();
		}
		else
		{
			PlayDefaultDialogue();
		}

	}


	void StartDialogue()
	{
		if (_startDialogueEvent != null)
		{
			_startDialogueEvent.RaiseEvent(_currentDialogue);
		}
	}
	void PlayLoseDialogue()
	{
		if (_questAnchor != null)
		{
			DialogueDataSO displayDialogue = _questAnchor.InteractWithCharacter(_actor, true, false);
			if (displayDialogue != null)
			{
				_currentDialogue = displayDialogue;
				StartDialogue();
			}

		}



	}
	void PlayWinDialogue()
	{
		if (_questAnchor != null)
		{
			DialogueDataSO displayDialogue = _questAnchor.InteractWithCharacter(_actor, true, true);
			if (displayDialogue != null)
			{
				_currentDialogue = displayDialogue;
				StartDialogue();
			}

		}
	}

}
