using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
public enum dialogueType
{
	startDialogue,
	winDialogue,
	loseDialogue,
	defaultDialogue,

}
/// <summary>
/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
/// In future versions it might contain support for branching conversations.
/// </summary>
[CreateAssetMenu(fileName = "newDialogue", menuName = "Dialogues/Dialogue Data")]
public class DialogueDataSO : ScriptableObject
{

	[SerializeField] private ActorSO _actor = default;
	[SerializeField] private List<DialogueLineSO> _dialogueLines;
	[SerializeField] private List<Choice> _choices;
	[SerializeField] private dialogueType _dialogueType;

	public ActorSO Actor => _actor; 
	public List<DialogueLineSO> DialogueLines => _dialogueLines; 
	public List<Choice> Choices => _choices;
	public dialogueType DialogueType => _dialogueType;

	//TODO: Add support for branching conversations
	// Maybe add 2 (or more) special line slots which represent a choice in a conversation
	// Each line would also have an event associated, or another Dialogue
}

[Serializable]
public class Choice
{
	[SerializeField] private DialogueLineSO _response;
	[SerializeField] private DialogueDataSO _nextDialogue;

	public DialogueLineSO Response { get => _response; }
	public DialogueDataSO NextDialogue { get => _nextDialogue; }
}
