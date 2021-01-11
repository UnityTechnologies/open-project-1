using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script needs to be put on the actor, and takes care of the current step to accomplish.
//the step contains a dialogue and maybe an event.

public class StepController : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private ActorSO _actor=default;
	[SerializeField] private DialogueDataSO _defaultDialogue = default;

	[Header("Listening to channels")]
	[SerializeField] private StepChannelSO _startStepEvent = default;
	[SerializeField] private VoidEventChannelSO _endDialogueEvent = default;
	[SerializeField] private DialogueActorChannelSO _interactionEvent = default;
	[SerializeField] private VoidEventChannelSO _winDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _loseDialogueEvent = default;
	[SerializeField] private StepChannelSO _endStepEvent = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private VoidEventChannelSO _checkStepValidityEvent = default;
	[SerializeField] private DialogueDataChannelSO _startDialogueEvent = default;

	//check if character is actif. An actif character is the character concerned by the step.
	private bool _hasActifStep;
	private StepSO _currentStep;
	private DialogueDataSO _currentDialogue;

	private void Start()
	{
		if (_endDialogueEvent != null)
		{ _endDialogueEvent.OnEventRaised += EndDialogue; }
		if (_startStepEvent != null)
		{ _startStepEvent.OnEventRaised += CheckStepInvolvment; }
		if (_interactionEvent != null)
		{ _interactionEvent.OnEventRaised += InteractWithCharacter; }
		if (_winDialogueEvent != null)
		{ _winDialogueEvent.OnEventRaised += PlayWinDialogue; }
		if (_loseDialogueEvent != null)
		{ _loseDialogueEvent.OnEventRaised += PlayLoseDialogue; }
		if (_endStepEvent != null)
		{ _endStepEvent.OnEventRaised += EndStep; }

	}
	//play default dialogue if no step
	void PlayDefaultDialogue()
	{
		if (_defaultDialogue != null)
		{
			_currentDialogue = _defaultDialogue; 
		    StartDialogue();
		}

	}
	void CheckStepInvolvment(StepSO step)
	{
		Debug.Log("check involvment"); 
		if(_actor == step.Actor)
		{
			RegisterStep(step); 
		}

	}
	//register a step
     void RegisterStep(StepSO step)
	{
		_currentStep = step;
		_hasActifStep = true;
		
	}
	//start a dialogue when interaction
	//some Steps need to be instantanious. And do not need the interact button.
	//when interaction again, restart same dialogue.
	 void InteractWithCharacter(ActorSO actorToInteractWith)
	{
		if (actorToInteractWith == _actor)
		{
			if (_hasActifStep)
			{
				StartStep();

			}
			else
			{
				PlayDefaultDialogue();
			}
		}
	}
	public void InteractWithCharacter()
	{
		if (_hasActifStep)
			{
				StartStep();

			}
			else
			{
				PlayDefaultDialogue();
			}
		
	}
	void StartStep() {
        if(_currentStep!=null)
		if (_currentStep.DialogueBeforeStep != null)
		{
			_currentDialogue = _currentStep.DialogueBeforeStep;
			StartDialogue();
		}
		else
		{
				Debug.LogError("step without dialogue registring not implemented."); 
		}
	}
	 void StartDialogue()
	{
		if (_startDialogueEvent != null)
		{
			Debug.Log("Start Dialogue ");
			_startDialogueEvent.RaiseEvent(_currentDialogue);
		}


	}
	void PlayLoseDialogue() {
		
		if (_currentStep != null)
			if (_currentStep.LoseDialogue != null)
			{
				Debug.Log("Play lose Dialogue ");
				_currentDialogue = _currentStep.LoseDialogue;
				StartDialogue();
			}
		
	}
	void PlayWinDialogue()
	{
		Debug.Log("Play Win Dialogue" + _currentStep.WinDialogue);
		if (_currentStep != null)
			if (_currentStep.WinDialogue != null)
			{
				_currentDialogue = _currentStep.WinDialogue;
				StartDialogue();
			}

	}
	//End dialogue
	 void EndDialogue()
	{
		
		//depending on the dialogue that ended, do something 
			switch (_currentDialogue.DialogueType)
			{
				case dialogueType.startDialogue:
					//Check the validity of the step
					CheckStepValidity();
					break;
				case dialogueType.winDialogue:
					//After playing the win dialogue close Dialogue and end step
					break;
				case dialogueType.loseDialogue:
					//closeDialogue
					//replay start Dialogue if the lose Dialogue ended
					if (_currentStep.DialogueBeforeStep != null)
					{
						_currentDialogue = _currentStep.DialogueBeforeStep;

					}
					break;
				case dialogueType.defaultDialogue:
					//close Dialogue
					//nothing happens if it's the default dialogue
					break;
				default:
					break;
			
		}
		


	}
	void CheckStepValidity()
	{
		if(_checkStepValidityEvent!=null)
		{
			_checkStepValidityEvent.RaiseEvent(); 
		}


	}
	 void EndStep(StepSO stepToFinish)
	{
		Debug.Log("End step " + stepToFinish.name); 
		if(stepToFinish==_currentStep)
		    UnregisterStep();
		else
		{
			StartStep(); 
		}

	}
	void EndStep()
	{
	
			UnregisterStep();

	}
	//unregister a step when it ends.
	void UnregisterStep()
	{
		_currentStep = null;
		_hasActifStep = false;
		_currentDialogue = _defaultDialogue; 

		
	}
}
