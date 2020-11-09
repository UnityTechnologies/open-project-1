using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUIController : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI lineText;

	[SerializeField]
	TextMeshProUGUI actorNameText;
	public void SetDialogue(DialogueLineSO dialogueLine)
	{

		lineText.text = dialogueLine.Sentence;
		actorNameText.text = dialogueLine.Actor.ActorName;


	}
}
