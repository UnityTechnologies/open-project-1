using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DialogueControlBehaviour : PlayableBehaviour
{
	public DialogueLineSO dialogueLine;
	public bool hasToPause; // Determines if the clip will pause the Timeline that contains it when it reaches the end

	public void DisplayDialogueLine()
	{
		//TODO: Interface with the DialogueManager and play the line of dialogue on screen
		Debug.Log(dialogueLine.Sentence);
		//TODO: Check if it has to pause the Timeline
	}
}
