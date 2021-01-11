using System.Collections.Generic;
using UnityEngine;
public enum stepType
{
	dialogue,
    giveItem,
	checkItem,
	rewardItem
}
[CreateAssetMenu(fileName = "step", menuName = "Quests/step", order = 51)]
public class StepSO : ScriptableObject
{

	[Tooltip("The Character this mission will need interaction with")]
	[SerializeField]
	private ActorSO _actor = default;
	[Tooltip("The dialogue that will be diplayed befor an action, if any")]
	[SerializeField]
	private DialogueDataSO _dialogueBeforeStep = default;
	[Tooltip("The dialogue that will be diplayed when the step is achieved")]
	[SerializeField]
	private DialogueDataSO _winDialogue = default;
	[Tooltip("The dialogue that will be diplayed if the step is not achieved yet")]
	[SerializeField]
	private DialogueDataSO _loseDialogue = default;
	[Tooltip("The item to check/give/reward")]
	[SerializeField]
	private Item _item = default;
	[Tooltip("The type of the step")]
	[SerializeField]
	private stepType _type = default;
	[SerializeField]
	bool _isDone=false;
	public DialogueDataSO DialogueBeforeStep => _dialogueBeforeStep;
	public DialogueDataSO WinDialogue => _winDialogue;
	public DialogueDataSO LoseDialogue => _loseDialogue;
	public Item Item => _item;
	public stepType Type => _type;
	public bool IsDone => _isDone;
	public ActorSO Actor => _actor;

	public void FinishStep()
	{

		_isDone = true; 

	}


}
