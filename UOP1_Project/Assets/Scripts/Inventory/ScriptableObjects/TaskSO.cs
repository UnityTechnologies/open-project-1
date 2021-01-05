using System.Collections.Generic;
using UnityEngine;
public enum taskType
{
	dialogue,
    giveItem,
	checkItem,
	rewardItem
}
[CreateAssetMenu(fileName = "Task", menuName = "Missions/Task", order = 51)]
public class TaskSO : ScriptableObject
{
	[SerializeField]
	private bool _isInstantanious;
	[Tooltip("The Character this mission will need interaction with")]
	[SerializeField]
	private ActorSO _actor;
	[SerializeField]
	private List <DialogueLineSO> _dialogue;
	[SerializeField]
	private Item _item;
	[SerializeField]
	private taskType _type;
	[SerializeField]
	VoidEventChannelSO _startDialogue;
	[SerializeField]
	VoidEventChannelSO _endDialogue;
	
	bool _isDone=false;
	public List<DialogueLineSO> Dialogue => _dialogue;
	public Item Item => _item;
	public taskType Type => _type;
	public bool IsDone => _isDone;
	public ActorSO Actor => _actor;
	public bool IsInstantanious => _isInstantanious; 
	public	VoidEventChannelSO StartDialogue => _startDialogue ;
public	VoidEventChannelSO EndDialogue=> _endDialogue;

	public void FinishTask()
	{

		_isDone = true; 

	}


}
