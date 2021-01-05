using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	public List<QuestSO> Quests =  new List<QuestSO>();
	public Inventory inventory;

	public ItemEventChannelSo GiveItemEvent;
	public ItemEventChannelSo RewardItemEvent;

	QuestSO currentQuest;
	TaskSO currentTask;
	int currentQuestIndex=0;
	int currentTaskIndex=0;
	private void Start()
	{
		EndTask(); 
	}
	public void StartQuest()
	{ 
		if( Quests.Count > currentQuestIndex)
		{
			currentQuest = Quests[currentQuestIndex];
			StartTask();
		}

	}

	public void StartTask()
	{
		if (currentQuest.Tasks.Count > currentTaskIndex)
		{
			currentTask = currentQuest.Tasks[currentTaskIndex];
			switch (currentTask.Type)
			{
				case taskType.checkItem:
					if (inventory.Contains(currentTask.Item))
					{
						inventory.Contains(currentTask.Item);
						EndTask();
					}
					else
					{ PlayDefaultDialogue(currentTask.Dialogue);  }
					break;
				case taskType.giveItem:
					if (inventory.Contains(currentTask.Item))
					{
						GiveItemEvent.RaiseEvent(currentTask.Item);
						EndTask();
					}
					else
					{ PlayDefaultDialogue(currentTask.Dialogue); }
					break;
				case taskType.rewardItem:
					RewardItemEvent.RaiseEvent(currentTask.Item);
					EndTask();
					break;
				case taskType.dialogue:
					StartDialogue(currentTask.Dialogue); 
					break;


			}

		}

	}
	public void StartDialogue (List<DialogueLineSO> Dialogue) {

		//check location 
		//find the actor
		//Add the dialogue on the actor 
		//subscribe to the dialogue end event

	}
	public void PlayDefaultDialogue(List<DialogueLineSO> Dialogue)
	{

		//check location 
		//find the actor
		//Add the dialogue on the actor 
		//subscribe to the dialogue end event

	}
	public void EndTask()
	{
		currentTask.FinishTask();
		Debug.Log(Quests[currentQuestIndex].Tasks[currentTaskIndex].IsDone); 

	}
	public void EndQuest()
	{




	}
}
