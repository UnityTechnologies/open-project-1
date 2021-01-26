using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Localization; 
public enum DialogueType
{
	startDialogue,
	winDialogue,
	loseDialogue,
	defaultDialogue,

}
public enum ChoiceActionType
{
	doNothing,
	continueWithStep

}
/// <summary>
/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
/// In future versions it might contain support for branching conversations.
/// </summary>
[CreateAssetMenu(fileName = "newDialogue", menuName = "Dialogues/Dialogue Data")]
public class DialogueDataSO : ScriptableObject
{

	[SerializeField] private ActorSO _actor = default;
	[SerializeField] private List<LocalizedString> _dialogueLines = default;
	[SerializeField] private List<Choice> _choices = default;
	[SerializeField] private DialogueType _dialogueType = default;

	public ActorSO Actor => _actor;
	public List<LocalizedString> DialogueLines => _dialogueLines;
	public List<Choice> Choices => _choices;
	public DialogueType DialogueType => _dialogueType;

	//TODO: Add support for branching conversations
	// Maybe add 2 (or more) special line slots which represent a choice in a conversation
	// Each line would also have an event associated, or another Dialogue
	private void OnEnable()
	{
		findDialogueLines();
	}
	void findDialogueLines()
	{
		Debug.Log(DialogueLines[0].TableEntryReference.Key); 
		Debug.Log(DialogueLines[0].TableReference.TableCollectionName);

	}
}

[Serializable]
public class Choice
{
	[SerializeField] private LocalizedString _response = default;
	[SerializeField] private DialogueDataSO _nextDialogue = default;
	[SerializeField] private ChoiceActionType _actionType = default;
	public LocalizedString Response => _response;
	public DialogueDataSO NextDialogue => _nextDialogue;
	public ChoiceActionType ActionType => _actionType;
}
