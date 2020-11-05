using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.</para>
/// <para>Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.</para>
/// </summary>
public class DialogueManager : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader;

	public void BeginDialogue(DialogueLineSO firstLine)
	{
		_inputReader.GameInput.Menus.Enable();

		DisplayDialogueLine(firstLine);
	}

    public void DisplayDialogueLine(DialogueLineSO dialogueLine)
	{
		//TODO: Interface with a UIManager to allow displayal of the line of dialogue in the UI
	}
}
