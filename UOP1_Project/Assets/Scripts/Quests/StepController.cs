using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//this script needs to be put on the actor, and takes care of the current step to accomplish.
//the step contains a dialogue and maybe an event.

public class StepController : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private ActorSO _actor = default;
	[SerializeField] private DialogueDataSO _defaultDialogue = default;
	[SerializeField] private QuestManagerSO _questData = default;
	[SerializeField] private GameStateSO _gameStateManager = default;

	[Header("Listening to channels")]
	[SerializeField] private VoidEventChannelSO _winDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _loseDialogueEvent = default;
	[SerializeField] private IntEventChannelSO _endDialogueEvent = default;

	[Header("Broadcasting on channels")]
	public DialogueDataChannelSO _startDialogueEvent = default;

	[Header("Dialogue Shot Camera")]
	public GameObject dialogueShot;

	//check if character is actif. An actif character is the character concerned by the step.
	private DialogueDataSO _currentDialogue;

	public bool isInDialogue; //Consumed by the state machine

	private void Start()
	{
		if (dialogueShot)
		{
			dialogueShot.transform.parent = null;
			dialogueShot.SetActive(false);
		}

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
		if (_gameStateManager.CurrentGameState == GameState.Gameplay)
		{
			DialogueDataSO displayDialogue = _questData.InteractWithCharacter(_actor, false, false);
			//Debug.Log("dialogue " + displayDialogue + "actor" + _actor);
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

	}

	void StartDialogue()
	{
		_startDialogueEvent.RaiseEvent(_currentDialogue);
		_endDialogueEvent.OnEventRaised += EndDialogue;
		StopConversation();
		_winDialogueEvent.OnEventRaised += PlayWinDialogue;
		_loseDialogueEvent.OnEventRaised += PlayLoseDialogue;
		isInDialogue = true;
		if (dialogueShot)
			dialogueShot.SetActive(true);
	}

	void EndDialogue(int dialogueType)
	{
		_endDialogueEvent.OnEventRaised -= EndDialogue;
		_winDialogueEvent.OnEventRaised -= PlayWinDialogue;
		_loseDialogueEvent.OnEventRaised -= PlayLoseDialogue;
		ResumeConversation();
		isInDialogue = false;
		if (dialogueShot)
			dialogueShot.SetActive(false);
	}

	void PlayLoseDialogue()
	{
		if (_questData != null)
		{
			DialogueDataSO displayDialogue = _questData.InteractWithCharacter(_actor, true, false);
			if (displayDialogue != null)
			{
				_currentDialogue = displayDialogue;
				StartDialogue();

			}

		}
	}

	void PlayWinDialogue()
	{
		if (_questData != null)
		{
			DialogueDataSO displayDialogue = _questData.InteractWithCharacter(_actor, true, true);
			if (displayDialogue != null)
			{
				_currentDialogue = displayDialogue;
				StartDialogue();
			}
		}
	}

	private void StopConversation()
	{
		GameObject[] talkingTo = gameObject.GetComponent<NPC>().talkingTo;
		if (talkingTo != null)
		{
			for (int i = 0; i < talkingTo.Length; ++i)
			{
				talkingTo[i].GetComponent<NPC>().npcState = NPCState.Idle;
			}
		}
	}

	private void ResumeConversation()
	{
		GameObject[] talkingTo = GetComponent<NPC>().talkingTo;
		if (talkingTo != null)
		{

			for (int i = 0; i < talkingTo.Length; ++i)
			{
				talkingTo[i].GetComponent<NPC>().npcState = NPCState.Talk;
			}
		}
	}
}
