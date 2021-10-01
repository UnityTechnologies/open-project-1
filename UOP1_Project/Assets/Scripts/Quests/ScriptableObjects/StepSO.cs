using UnityEditor;
using UnityEngine;
public enum StepType
{
	Dialogue,
	GiveItem,
	CheckItem
}
[CreateAssetMenu(fileName = "step", menuName = "Quests/Step")]
public class StepSO : SerializableScriptableObject
{
	[Tooltip("The Character this mission will need interaction with")]
	[SerializeField] private ActorSO _actor = default;
	[Tooltip("The dialogue that will be diplayed befor an action, if any")]
	[SerializeField] private DialogueDataSO _dialogueBeforeStep = default;
	[Tooltip("The dialogue that will be diplayed when the step is achieved")]
	[SerializeField] private DialogueDataSO _completeDialogue = default;
	[Tooltip("The dialogue that will be diplayed if the step is not achieved yet")]
	[SerializeField] private DialogueDataSO _incompleteDialogue = default;
	[SerializeField] private StepType _type = default;
	[Tooltip("The item to check/give")]
	[SerializeField] private ItemSO _item = default;
	[SerializeField] private bool _hasReward = default;
	[Tooltip("The item to reward if any")]
	[SerializeField] private ItemSO _rewardItem = default;
	[SerializeField] private int _rewardItemCount = 1; // by default the reward is 1 item (if any)
	[SerializeField] bool _isDone = false;
	[SerializeField] VoidEventChannelSO _endStepEvent = default;

	public DialogueDataSO DialogueBeforeStep
	{
		get { return _dialogueBeforeStep; }
		set { _dialogueBeforeStep = value; }
	}
	public DialogueDataSO CompleteDialogue
	{
		get { return _completeDialogue; }
		set { _completeDialogue = value; }
	}
	public DialogueDataSO IncompleteDialogue
	{
		get { return _incompleteDialogue; }
		set { _incompleteDialogue = value; }
	}
	public ItemSO Item
	{
		get => _item;
		set => _item = value;
	}
	public bool HasReward => _hasReward;
	public ItemSO RewardItem => _rewardItem;
	public int RewardItemCount => _rewardItemCount;
	public VoidEventChannelSO EndStepEvent
	{
		set => _endStepEvent = value;
		get => _endStepEvent;
	}
	public StepType Type => _type;
	public bool IsDone
	{
		get => _isDone;
		set => _isDone = value;
	}
	public ActorSO Actor => _actor;

	public void FinishStep()
	{
		if (_endStepEvent != null)
			_endStepEvent.RaiseEvent();
		_isDone = true;
	}

	//This function is a leftover of the QuestEditorWindow, which is currently non functional
	public DialogueDataSO StepToDialogue()
	{
		DialogueDataSO dialogueData = ScriptableObject.CreateInstance<DialogueDataSO>();
		/*
				dialogueData.SetActor(Actor);
				if (DialogueBeforeStep != null)
				{
					 dialogueData = new DialogueDataSO(DialogueBeforeStep);
					if (DialogueBeforeStep.Choices != null)
					{
						if (CompleteDialogue != null)
						{
							if (dialogueData.Choices.Count > 0)
							{

								if (dialogueData.Choices[0].NextDialogue == null)
									dialogueData.Choices[0].SetNextDialogue(CompleteDialogue);
							}
						}
						if (IncompleteDialogue != null)
						{
							if (dialogueData.Choices.Count > 1)
							{
								if (dialogueData.Choices[1].NextDialogue == null)
									dialogueData.Choices[1].SetNextDialogue(IncompleteDialogue);
							}

						}

					}

				}

				*/
		return dialogueData;
	}

#if UNITY_EDITOR
	/// <summary>
	/// This function is only useful for the Questline Tool in Editor to remove a Step
	/// </summary>
	/// <returns>The local path</returns>
	public string GetPath()
	{
		return AssetDatabase.GetAssetPath(this);
	}
#endif
}
