using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

/// <summary>
/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
/// In future versions it might contain support for branching conversations.
/// </summary>
[CreateAssetMenu(fileName = "newDialogue", menuName = "CutsceneSystem/Dialogue Data")]
public class DialogueDataSO : ScriptableObject
{
	public List<DialogueLineSO> dialogueLines;

	//TODO: Add support for branching conversations
	// Maybe add 2 (or more) special line slots which represent a choice in a conversation
	// Each line would also have an event associated, or another Dialogue
}
