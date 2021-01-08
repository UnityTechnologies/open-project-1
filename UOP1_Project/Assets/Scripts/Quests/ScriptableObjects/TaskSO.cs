using System.Collections.Generic;
using UnityEngine;
public enum taskType
{
	dialogue,
    giveItem,
	checkItem,
	rewardItem
}
[CreateAssetMenu(fileName = "Task", menuName = "Quests/Task", order = 51)]
public class TaskSO : ScriptableObject
{

	[Tooltip("The Character this mission will need interaction with")]
	[SerializeField]
	private ActorSO _actor = default;
	[Tooltip("The dialogue that will be diplayed befor an action, if any")]
	[SerializeField]
	private DialogueDataSO _dialogueBeforeTask = default;
	[Tooltip("The dialogue that will be diplayed when the task is achieved")]
	[SerializeField]
	private DialogueDataSO _winDialogue = default;
	[Tooltip("The dialogue that will be diplayed if the task is not achieved yet")]
	[SerializeField]
	private DialogueDataSO _loseDialogue = default;
	[Tooltip("The item to check/give/reward")]
	[SerializeField]
	private Item _item = default;
	[Tooltip("The type of the task")]
	[SerializeField]
	private taskType _type = default;
	
	bool _isDone=false;
	public DialogueDataSO DialogueBeforeTask => _dialogueBeforeTask;
	public DialogueDataSO WinDialogue => _winDialogue;
	public DialogueDataSO LoseDialogue => _loseDialogue;
	public Item Item => _item;
	public taskType Type => _type;
	public bool IsDone => _isDone;
	public ActorSO Actor => _actor;

	public void FinishTask()
	{

		_isDone = true; 

	}


}
