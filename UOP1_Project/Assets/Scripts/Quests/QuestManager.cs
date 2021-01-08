using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private List<QuestSO> _quests = default;
	[SerializeField] private Inventory _inventory = default;


	[Header("Linstening to channels")]
	[SerializeField] private VoidEventChannelSO _checkTaskValidityEvent = default;
	[SerializeField] private VoidEventChannelSO _endTaskEvent = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private TaskChannelSO _startTaskEvent = default;

	[SerializeField] private VoidEventChannelSO _winDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _loseDialogueEvent = default;

	[SerializeField] private ItemEventChannelSo _giveItemEvent = default;
	[SerializeField] private ItemEventChannelSo _rewardItemEvent = default; 

	private QuestSO _currentQuest;
	private TaskSO _currentTask;
	private int _currentQuestIndex =0;
	private int _currentTaskIndex =0;
	private void Start()
 	{
	    if(	_checkTaskValidityEvent!=null)
		{
			_checkTaskValidityEvent.OnEventRaised += CheckTaskValidity; 
		}
		if (_endTaskEvent != null)
		{
			_endTaskEvent.OnEventRaised += EndTask;
		}
		StartGame(); 

	}
	 void StartGame()
	{//Add code for saved information 
		_currentQuestIndex = 0;
		if (_quests != null)
		{
			_currentQuestIndex = _quests.FindIndex(o => o.IsDone == false);
			if(_currentQuestIndex >0)
			StartQuest();
		}

	}
	 void StartQuest()
	{
		if (_quests != null)
			if ( _quests.Count > _currentQuestIndex)
		{
			_currentQuest = _quests[_currentQuestIndex];
			_currentTaskIndex = 0;
			_currentTaskIndex = _currentQuest.Tasks.FindIndex(o => o.IsDone == false);
				if (_currentTaskIndex > 0)
					StartTask();
		}

	}

	 void StartTask()
	{
		if(_currentQuest.Tasks!=null)
			if (_currentQuest.Tasks.Count > _currentTaskIndex)
		    {
			_currentTask = _currentQuest.Tasks[_currentTaskIndex];
			_startTaskEvent.RaiseEvent(_currentTask); 

		    }

	}
	void CheckTaskValidity()
	{
		if(_currentQuest!=null)
			if (_currentQuest.Tasks != null)
				if (_currentQuest.Tasks.Count > _currentTaskIndex)
		{
			_currentTask = _currentQuest.Tasks[_currentTaskIndex];
			switch (_currentTask.Type)
			{
				case taskType.checkItem:
					if (_inventory.Contains(_currentTask.Item))
					{
						_inventory.Contains(_currentTask.Item);
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
				case taskType.giveItem:
					if (_inventory.Contains(_currentTask.Item))
					{
						_giveItemEvent.RaiseEvent(_currentTask.Item);
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
				case taskType.rewardItem:
					_rewardItemEvent.RaiseEvent(_currentTask.Item);
					//no dialogue is needed after Reward Item
					break;
				case taskType.dialogue:
					//dialogue has already been played
					break;


			}
		}


		}

	 void EndTask()
	{
		_currentTask.FinishTask();

		if (_quests != null)
			if (_quests.Count > _currentQuestIndex)
				if (_quests[_currentQuestIndex].Tasks != null)
					if (_quests[_currentQuestIndex].Tasks.Count > _currentTaskIndex)
					{
						Debug.Log(_quests[_currentQuestIndex].Tasks[_currentTaskIndex].IsDone);
						_quests[_currentQuestIndex].Tasks[_currentTaskIndex].FinishTask();

						if(_quests[_currentQuestIndex].Tasks.Count < _currentTaskIndex+1)
						{
							_currentTaskIndex++;
							StartTask();

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
