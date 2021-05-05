using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//[CreateAssetMenu(fileName = "QuestManager", menuName = "Quests/QuestManager", order = 51)]
public class QuestManagerSO : ScriptableObject
{
	[Header("Data")]
	[SerializeField] private List<QuestlineSO> _questlines = default;
	[SerializeField] private Inventory _inventory = default;



	[Header("Linstening to channels")]
	[SerializeField] private VoidEventChannelSO _checkStepValidityEvent = default;
	[SerializeField] private DialogueDataChannelSO _endDialogueEvent = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private VoidEventChannelSO _completeDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _incompleteDialogueEvent = default;

	[SerializeField] private ItemEventChannelSO _giveItemEvent = default;
	[SerializeField] private ItemEventChannelSO _rewardItemEvent = default;

	private QuestSO _currentQuest = null;
	private QuestlineSO _currentQuestline;
	private StepSO _currentStep;
	private int _currentQuestIndex = 0;
	private int _currentQuestlineIndex = 0;
	private int _currentStepIndex = 0;

	public void StartGame()
	{//Add code for saved information
			_checkStepValidityEvent.OnEventRaised += CheckStepValidity;
			_endDialogueEvent.OnEventRaised += EndDialogue;
		
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
	bool HasStep(ActorSO actorToCheckWith)
	{
		if (_currentStep != null)
		{

			if (_currentStep.Actor == actorToCheckWith)
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
	{	if (_currentQuest == null)
		{
			if (CheckQuestlineForQuestWithActor(actor))
			{
				StartQuest(actor);
			}

		}

		if (HasStep(actor))
		{
			if (isCheckValidity)
			{

				if (isValid)
				{
					return _currentStep.CompleteDialogue;

				}
				else
				{
					return _currentStep.IncompleteDialogue;

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

		if (_currentStep != null)
		{
			switch (_currentStep.Type)
			{
				case StepType.CheckItem:

					if (_inventory.Contains(_currentStep.Item))
					{
						_inventory.Contains(_currentStep.Item);
						//Trigger win dialogue
						_completeDialogueEvent.RaiseEvent();
					}
					else
					{
						//trigger lose dialogue
						_incompleteDialogueEvent.RaiseEvent();
					}
					break;
				case StepType.GiveItem:
					if (_inventory.Contains(_currentStep.Item))
					{
						_giveItemEvent.RaiseEvent(_currentStep.Item);
						_completeDialogueEvent.RaiseEvent();
					}
					else
					{
						//trigger lose dialogue
						_incompleteDialogueEvent.RaiseEvent();

					}
					break;
				case StepType.RewardItem:
					_rewardItemEvent.RaiseEvent(_currentStep.Item);
					//no dialogue is needed after Reward Item
					if (_currentStep.CompleteDialogue != null)
					{
						_completeDialogueEvent.RaiseEvent();
					}
					else
					{
						EndStep();
					}
					break;
				case StepType.Dialogue:
					//dialogue has already been played
					if (_currentStep.CompleteDialogue != null)
					{

						_completeDialogueEvent.RaiseEvent();
					}
					else
					{
						EndStep();
					}
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



