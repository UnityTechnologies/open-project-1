using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private List<QuestSO> _quests = default;
	[SerializeField] private Inventory _inventory = default;


	[Header("Linstening to channels")]
	[SerializeField] private VoidEventChannelSO _checkStepValidityEvent = default;
	[SerializeField] private StepChannelSO _endStepEvent = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private StepChannelSO _startStepEvent = default;

	[SerializeField] private VoidEventChannelSO _winDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _loseDialogueEvent = default;

	[SerializeField] private ItemEventChannelSO _giveItemEvent = default;
	[SerializeField] private ItemEventChannelSO _rewardItemEvent = default; 

	private QuestSO _currentQuest;
	private StepSO _currentStep;
	private int _currentQuestIndex =0;
	private int _currentStepIndex =0;
	private void Start()
	{
		Debug.Log("Start");
		if (	_checkStepValidityEvent!=null)
		{
			_checkStepValidityEvent.OnEventRaised += CheckStepValidity; 
		}
		
		StartGame(); 

	}
	 void StartGame()
	{//Add code for saved information
		Debug.Log("Start Game"); 
		_currentQuestIndex = 0;
		if (_quests != null)
		{
			_currentQuestIndex = _quests.FindIndex(o => o.IsDone == false);
			if (_currentQuestIndex >=0)
			StartQuest();
		}

	}
	 void StartQuest()
	{
		Debug.Log("Start Quest");
		if (_quests != null)
			if ( _quests.Count > _currentQuestIndex)
		{
			_currentQuest = _quests[_currentQuestIndex];
			_currentStepIndex = 0;
			_currentStepIndex = _currentQuest.Steps.FindIndex(o => o.IsDone == false);
				if (_currentStepIndex >= 0)
					StartStep();
		}

	}

	 void StartStep()
	{
		Debug.Log("Start step");
		if (_currentQuest.Steps!=null)
			if (_currentQuest.Steps.Count > _currentStepIndex)
		    {
			_currentStep = _currentQuest.Steps[_currentStepIndex];
			_startStepEvent.RaiseEvent(_currentStep); 

		    }

	}
	void CheckStepValidity()
	{
		if(_currentQuest!=null)
			if (_currentQuest.Steps != null)
				if (_currentQuest.Steps.Count > _currentStepIndex)
		{
			_currentStep = _currentQuest.Steps[_currentStepIndex];
			switch (_currentStep.Type)
			{
				case stepType.checkItem:
					if (_inventory.Contains(_currentStep.Item))
					{
						_inventory.Contains(_currentStep.Item);
						//Trigger win dialogue
						if(_winDialogueEvent!=null)
						{
							_winDialogueEvent.OnEventRaised();
						}
					}
					else
					{
						//trigger lose dialogue
						if (_loseDialogueEvent != null)
						{
							_loseDialogueEvent.OnEventRaised();
						}
					}
					break;
				case stepType.giveItem:
					if (_inventory.Contains(_currentStep.Item))
					{
						_giveItemEvent.RaiseEvent(_currentStep.Item);
						//trigger win dialogue
						if (_winDialogueEvent != null)
						{
							_winDialogueEvent.OnEventRaised();
						}
							}
					else
					{
						//trigger lose dialogue
						if (_loseDialogueEvent != null)
						{
							_loseDialogueEvent.OnEventRaised();
						}
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

	 void EndStep()
	{
		_currentStep = null; 

		if (_quests != null)
			if (_quests.Count > _currentQuestIndex)
				if (_quests[_currentQuestIndex].Steps != null)
					if (_quests[_currentQuestIndex].Steps.Count > _currentStepIndex)
					{
						if (_endStepEvent != null)
							_endStepEvent.RaiseEvent(_quests[_currentQuestIndex].Steps[_currentStepIndex]);
						_quests[_currentQuestIndex].Steps[_currentStepIndex].FinishStep();
						if(_quests[_currentQuestIndex].Steps.Count > _currentStepIndex +1)
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

		if (_quests != null)
			if (_quests.Count > _currentQuestIndex)
			{
						_quests[_currentQuestIndex].FinishQuest();

				if (_quests.Count < _currentQuestIndex + 1)
				{
					_currentQuestIndex++;
					StartQuest();

				}
						
			}


	}
}
