using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TownsfolkState { Idle = 0, Walk, Talk };


public class Townsfolk : MonoBehaviour
{
	public TownsfolkState townsfolkState; //This is checked by conditions in the StateMachine
	public GameObject[] talkingTo;

	private DialogueDataChannelSO _startDialogueEvent = default;
	private VoidEventChannelSO _endDialogueEvent = default;

	private void OnEnable()
	{
		if (_startDialogueEvent != null)
			_startDialogueEvent.OnEventRaised += StopTalking;

		if (_endDialogueEvent != null)
			_endDialogueEvent.OnEventRaised += ResumeTalking;
	}


	private void StopTalking(DialogueDataSO dialogue)
    {
		for (int i = 0; i < talkingTo.Length; ++i)
		{
			if (talkingTo[i].GetComponent<StepController>().IsInDialogue)
			{
				townsfolkState = TownsfolkState.Idle;
				return;
			}
		}
	}

	private void ResumeTalking()
	{
		for (int i = 0; i < talkingTo.Length; ++i)
		{
			if (talkingTo[i].GetComponent<StepController>().IsInDialogue)
				return;
		}
		townsfolkState = TownsfolkState.Talk;

	}
}
