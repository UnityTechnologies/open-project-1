using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "dialogueEvent", menuName = "UI Event/dialogueLineEvent")]
public class DialogueLineEvent : ScriptableObject
{

	public UnityAction<DialogueLineSO> eventRaised;
	public void Raise(DialogueLineSO dialogueLine)
	{
		if (eventRaised != null)
			eventRaised.Invoke(dialogueLine);
	}

}
