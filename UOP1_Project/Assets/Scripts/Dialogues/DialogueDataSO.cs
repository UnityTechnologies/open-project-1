using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
<<<<<<< Updated upstream
=======
using UnityEngine.Localization;
using UnityEditor.Localization;

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
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
=======
	private void OnEnable()
	{
		FindDialogueLines(); 
	}
	void FindDialogueLines()
	{
		int index = 0;
		var collection = LocalizationEditorSettings.GetStringTableCollection("NewQuestlineDialogue");
        _dialogueLines.Clear();
		if (collection != null)
		{
			LocalizedString _dialogueLine = null;
			do
			{
				index++;
				var key = "L" + index + "-" + this.name;



				if (collection.SharedData.Contains(key))
				{
					Debug.Log("found Key " + key);
					_dialogueLine = new LocalizedString() { TableReference = "NewQuestlineDialogue", TableEntryReference = key };
					_dialogueLines.Add(_dialogueLine);
				}
				else
				{
					Debug.Log("No Key " + key);
					_dialogueLine = null;

				}
			} while (_dialogueLine != null);


		}

		
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
>>>>>>> Stashed changes
}
