using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor.Localization;
using UnityEditor;
#endif

public enum DialogueType
{
	StartDialogue,
	CompletionDialogue,
	IncompletionDialogue,
	DefaultDialogue,
}

public enum ChoiceActionType
{
	DoNothing,
	ContinueWithStep,
	WinningChoice,
	LosingChoice,
	IncompleteStep
}

/// <summary>
/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
/// In future versions it might contain support for branching conversations.
/// </summary>
[CreateAssetMenu(fileName = "new Dialogue", menuName = "Dialogues/Dialogue Data")]
public class DialogueDataSO : ScriptableObject
{
	[SerializeField] private List<Line> _lines = default;
	[SerializeField] private DialogueType _dialogueType = default;
	[SerializeField] private VoidEventChannelSO _endOfDialogueEvent = default;
	
	public VoidEventChannelSO EndOfDialogueEvent => _endOfDialogueEvent;
	public List<Line> Lines => _lines;

	public DialogueType DialogueType
	{
		get { return _dialogueType; }
		set { _dialogueType = value; }
	}

	public void FinishDialogue()
	{
		if (EndOfDialogueEvent != null)
			EndOfDialogueEvent.RaiseEvent();
	}

#if UNITY_EDITOR
	private void OnEnable()
	{
		SetDialogueLines(this.name);
	}
	public DialogueDataSO(string dialogueName)
	{
		SetDialogueLines(dialogueName);
	}
	void SetDialogueLines(string dialogueName)
	{
		if (_lines == null)
			_lines = new List<Line>();

		_lines.Clear();
		int dialogueIndex = 0;
		Line _dialogueLine = new Line();

		do
		{
			dialogueIndex++;
			_dialogueLine = new Line("D" + dialogueIndex + "-" + dialogueName);
			if (_dialogueLine.TextList != null)
				_lines.Add(_dialogueLine);

		} while (_dialogueLine.TextList != null);
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

	public Choice(Choice choice)
	{
		_response = choice.Response;
		_nextDialogue = choice.NextDialogue;
		_actionType = ActionType;
	}

	public Choice(LocalizedString response)
	{
		_response = response;
	}

	public void SetChoiceAction(Comment comment)
	{
		_actionType = (ChoiceActionType)Enum.Parse(typeof(ChoiceActionType), comment.CommentText);
	}
}

[Serializable]
public class Line
{
	[SerializeField] private ActorID _actorID = default;
	[SerializeField] private List<LocalizedString> _textList = default;
	[SerializeField] private List<Choice> _choices = null;

	public ActorID Actor => _actorID;
	public List<LocalizedString> TextList => _textList;
	public List<Choice> Choices => _choices;

	public Line()
	{
		_textList = null;
	}

	public void SetActor(Comment comment)
	{
		_actorID = (ActorID)Enum.Parse(typeof(ActorID), comment.CommentText);
	}

#if UNITY_EDITOR
	public Line(string _name)
	{
		StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection("Questline Dialogue");
		_textList = null;
		if (collection != null)
		{

			int lineIndex = 0;
			LocalizedString _dialogueLine = null;
			do
			{
				lineIndex++;
				string key = "L" + lineIndex + "-" + _name;
				if (collection.SharedData.Contains(key))
				{

					SetActor(collection.SharedData.GetEntry(key).Metadata.GetMetadata<Comment>());
					_dialogueLine = new LocalizedString() { TableReference = "Questline Dialogue", TableEntryReference = key };
					if (_textList == null)
						_textList = new List<LocalizedString>();
					_textList.Add(_dialogueLine);
				}
				else
				{
					_dialogueLine = null;
				}

			} while (_dialogueLine != null);

			int choiceIndex = 0;
			Choice choice = null;
			do
			{
				choiceIndex++;
				string key = "C" + choiceIndex + "-" + _name;

				if (collection.SharedData.Contains(key))
				{
					LocalizedString _choiceLine = new LocalizedString() { TableReference = "Questline Dialogue", TableEntryReference = key };
					choice = new Choice(_choiceLine);
					choice.SetChoiceAction(collection.SharedData.GetEntry(key).Metadata.GetMetadata<Comment>());

					if (_choices == null)
					{
						_choices = new List<Choice>();
					}
					_choices.Add(choice);
				}
				else
				{
					choice = null;
				}
			} while (choice != null);
		}
		else
		{
			_textList = null;
		}
	}
#endif
}
