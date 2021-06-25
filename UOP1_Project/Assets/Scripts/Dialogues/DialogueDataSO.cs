using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Localization;
using UnityEditor.Localization;
using UnityEditor;

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
	continueWithStep,
	winningChoice,
	losingChoice,

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
	public DialogueType DialogueType
	{
		get { return _dialogueType; }
		set { _dialogueType = value; }
	}

	public void SetActor(ActorSO newActor)
	{
		_actor = newActor;

	}
	public DialogueDataSO()
	{


	}
	public DialogueDataSO(DialogueDataSO dialogue)
	{

		_actor = dialogue.Actor;
		_dialogueLines = new List<LocalizedString>(dialogue.DialogueLines);
		_choices = new List<Choice>();
		if(dialogue.Choices!=null)
		for (int i=0; i<dialogue.Choices.Count; i++)
		{

			_choices.Add(new Choice(dialogue.Choices[i])); 

		}
		_dialogueType = dialogue.DialogueType;

	}
#if UNITY_EDITOR

	//TODO: Add support for branching conversations
	// Maybe add 2 (or more) special line slots which represent a choice in a conversation
	// Each line would also have an event associated, or another Dialogue
	private void OnEnable()
	{
		SetDialogueLines();
	}
	void SetDialogueLines()
	{
		if (_dialogueLines == null)
			_dialogueLines = new List<LocalizedString>();
		_dialogueLines.Clear();

		StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection("Questline Dialogue");

		if (collection != null)
		{
			int index = 0;
			LocalizedString _dialogueLine = null;

			do
			{
				index++;
				string key = "L" + index + "-" + this.name;

				if (collection.SharedData.Contains(key))
				{
					_dialogueLine = new LocalizedString() { TableReference = "Questline Dialogue", TableEntryReference = key };
					_dialogueLines.Add(_dialogueLine);
				}
				else
				{
					_dialogueLine = null;
				}

			} while (_dialogueLine != null);

		}
	}
public void CreateLine()
	{
		if (_dialogueLines == null)
			_dialogueLines = new List<LocalizedString>();
		_dialogueLines.Clear(); 
		StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection("Questline Dialogue");

		if (collection != null)
		{

			string DefaultKey = "L" + 1 + "-" + this.name;
			if (!collection.SharedData.Contains(DefaultKey))
			{

				collection.SharedData.AddKey(DefaultKey);


			}
		}
		SetDialogueLines(); 
		}
	public void RemoveLineFromSharedTable()
	{
		
		StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection("Questline Dialogue");

		if (collection != null)
		{
			int index = 0;
			LocalizedString _dialogueLine = null;


			

			do
			{
				index++;
				string key = "L" + index + "-" + this.name;

				if (collection.SharedData.Contains(key))
				{
					collection.SharedData.RemoveKey(key); 
				}
				else
				{
					_dialogueLine = null;
				}

			} while (_dialogueLine != null);

		}
	}

	/// <summary>
	/// This function is only useful for the Questline Tool in Editor to remove a Questline
	/// </summary>
	/// <returns>The local path</returns>
	public string GetPath()
	{
		return AssetDatabase.GetAssetPath(this);
	}
#endif

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
	public void SetNextDialogue(DialogueDataSO dialogue)
	{
		_nextDialogue = dialogue;
	}
	public Choice()
	{


	}
	public Choice(Choice choice)
	{
		_response = choice.Response;
		_nextDialogue = choice.NextDialogue;
		_actionType = ActionType;
	}
}
