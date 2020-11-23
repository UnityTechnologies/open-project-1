using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.
/// Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.
/// </summary>
public class DialogueManager : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader = default;

	/// <summary>
	/// Called to begin a dialogue, i.e. a sequence of lines that require the player's input to move forward.
	/// </summary>
	/// <param name="firstLine"></param>
	public void BeginDialogue(DialogueLineSO firstLine)
	{
		_inputReader.EnableDialogueInput();
		DisplayDialogueLine(firstLine);
	}

	/// <summary>
	/// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
	/// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
	/// </summary>
	/// <param name="dialogueLine"></param>
	public void DisplayDialogueLine(DialogueLineSO dialogueLine)
	{
		//TODO: Interface with a UIManager to allow displayal of the line of dialogue in the UI
		//Debug.Log("A line of dialogue has been spoken: \"" + dialogueLine.Sentence + "\" by " + dialogueLine.Actor.ActorName);
		UIManager.Instance.OpenUIDialogue(dialogueLine);

	}
}
