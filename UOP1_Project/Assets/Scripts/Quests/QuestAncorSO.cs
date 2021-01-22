using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestAnchor", menuName = "Quests/QuestAnchor", order = 51)]
public class QuestAncorSO : ScriptableObject
{
	[Header("Data")]
	[SerializeField] private List<QuestlineSO> _questlines = default;
	[SerializeField] private Inventory _inventory = default;



	[Header("Linstening to channels")]
	[SerializeField] private VoidEventChannelSO _checkStepValidityEvent = default;
	[SerializeField] private DialogueDataChannelSO _endDialogueEvent = default;
	//[SerializeField] private DialogueActorChannelSO _checkForQuest = default;

	[Header("Broadcasting on channels")]
	//[SerializeField] private StepChannelSO _startStepEvent = default;

	//[SerializeField] private VoidEventChannelSO _endStepEvent = default;

	[SerializeField] private VoidEventChannelSO _winDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _loseDialogueEvent = default;

	[SerializeField] private ItemEventChannelSO _giveItemEvent = default;
	[SerializeField] private ItemEventChannelSO _rewardItemEvent = default;

	private QuestSO _currentQuest=null;
	private QuestlineSO _currentQuestline;
	private StepSO _currentStep;
	private int _currentQuestIndex = 0;
	private int _currentQuestlineIndex = 0;
	private int _currentStepIndex = 0;
	
public	void StartGame()
	{//Add code for saved information
		if (_checkStepValidityEvent != null)
		{
			_checkStepValidityEvent.OnEventRaised += CheckStepValidity;
		}
		if (_endDialogueEvent != null)
		{
			_endDialogueEvent.OnEventRaised += EndDialogue;
		}
		StartQuestline();
	}
	void StartQuestline()
	{
				if (_questlines != null)
		{
			if (_questlines.Exists(o => !o.IsDone))
			{
				_currentQuestlineIndex = _questlines.FindIndex(o => !o.IsDone);
				if (_currentQuestlineIndex >= 0)
					_currentQuestline = _questlines.Find(o => !o.IsDone);
			}

		}
	}
	bool hasStep(ActorSO actorToCheckWith)
	{
		if(_currentStep!=null)
		{

			if(_currentStep.Actor== actorToCheckWith)
			{
				return true; 
			}

		}
		return false; 

	}
	bool CheckQuestlineForQuestWithActor(ActorSO actorToCheckWith)
	{
		if (_currentQuest == null)//check if there's a current quest 
		{
			if (_currentQuestline != null)
			{
				
				return _currentQuestline.Quests.Exists(o => !o.IsDone && o.Steps != null && o.Steps[0].Actor == actorToCheckWith);

			}

		}
		return false;
	}

	public DialogueDataSO InteractWithCharacter(ActorSO actor, bool isCheckValidity, bool isValid)
	{
		
		if (_currentQuest == null)
		{
			if (CheckQuestlineForQuestWithActor(actor))
			{
				StartQuest(actor);
			}

		}

		if (hasStep(actor))
		{
			if (isCheckValidity)
			{

				if (isValid)
				{
					return _currentStep.WinDialogue;

				}
				else
				{
					return _currentStep.LoseDialogue;

				}

			}
			else
			{
				return _currentStep.DialogueBeforeStep; 
			}

		}
		return null;

	}
	//When Interacting with a character, we ask the quest manager if there's a quest that starts with a step with a certain character
	void StartQuest(ActorSO actorToCheckWith)
	{
		if (_currentQuest != null)//check if there's a current quest 
		{
			return;
		}

		if (_currentQuestline != null)
		{
			//find quest index
			_currentQuestIndex = _currentQuestline.Quests.FindIndex(o => !o.IsDone && o.Steps != null && o.Steps[0].Actor == actorToCheckWith);

			if ((_currentQuestline.Quests.Count > _currentQuestIndex) && (_currentQuestIndex >= 0))
			{
				_currentQuest = _currentQuestline.Quests[_currentQuestIndex];
				//start Step
				_currentStepIndex = 0;
				_currentStepIndex = _currentQuest.Steps.FindIndex(o => o.IsDone == false);
				if (_currentStepIndex >= 0)
					StartStep();
			}
		}

	}

	void StartStep()
	{
		if (_currentQuest.Steps != null)
			if (_currentQuest.Steps.Count > _currentStepIndex)
			{
				_currentStep = _currentQuest.Steps[_currentStepIndex];

			}

	}
	 void CheckStepValidity()
	{
		Debug.Log("Check step Validity");

		if (_currentStep != null)
		{
			switch (_currentStep.Type)
			{
				case stepType.checkItem:
					
					if (_inventory.Contains(_currentStep.Item))
					{
						_inventory.Contains(_currentStep.Item);
						//Trigger win dialogue
						_winDialogueEvent.RaiseEvent();
					}
					else
					{
						//trigger lose dialogue
						_loseDialogueEvent.RaiseEvent();
					}
					break;
				case stepType.giveItem:
					Debug.Log("Check Item ");
					if (_inventory.Contains(_currentStep.Item))
					{
						Debug.Log("Item exists");
						_giveItemEvent.RaiseEvent(_currentStep.Item);
						_winDialogueEvent.RaiseEvent();
					}
					else
					{
						//trigger lose dialogue
						_loseDialogueEvent.RaiseEvent();

					}
					break;
				case stepType.rewardItem:
					_rewardItemEvent.RaiseEvent(_currentStep.Item);
					//no dialogue is needed after Reward Item
					EndStep();
					break;
				case stepType.dialogue:
					//dialogue has already been played
					EndStep();
					break;


			}
		}
	}
	void EndDialogue(DialogueDataSO dialogue)
	{

		//depending on the dialogue that ended, do something 
		switch (dialogue.DialogueType)
		{
			case DialogueType.winDialogue:
				EndStep();
				break;
			case DialogueType.startDialogue:
				CheckStepValidity();
				break;
			default:
				break;

		}

	}
	void EndStep()
	{
		
		_currentStep = null;

		if (_currentQuest != null)
			if (_currentQuest.Steps.Count > _currentStepIndex)
			{
				_currentQuest.Steps[_currentStepIndex].FinishStep();
				if (_currentQuest.Steps.Count > _currentStepIndex + 1)
				{
					_currentStepIndex++;
					StartStep();

				}
				else
				{ 

				EndQuest();
				}
			}



	}
	void EndQuest()
	{

		if (_currentQuest != null)
			_currentQuest.FinishQuest();

		_currentQuest = null;
		_currentQuestIndex = -1;
		if (_currentQuestline != null)
		{
			if (!_currentQuestline.Quests.Exists(o => !o.IsDone))
			{
				EndQuestline();

			}

		}


	}
	void EndQuestline()
	{
		if (_questlines != null)
		{
			if (_currentQuestline != null)
			{
				_currentQuestline.FinishQuestline();

			}

			if (_questlines.Exists(o => o.IsDone))
			{
				StartQuestline();

			}

		}


	}
}



