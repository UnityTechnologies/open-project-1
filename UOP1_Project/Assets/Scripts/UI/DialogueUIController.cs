using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components; 
public class DialogueUIController : MonoBehaviour
{
	[SerializeField]
	LocalizeStringEvent lineText;

	[SerializeField]
	LocalizeStringEvent actorNameText;
	public void SetDialogue(DialogueLineSO dialogueLine)
	{

		lineText.StringReference = dialogueLine.Sentence;
		actorNameText.StringReference = dialogueLine.Actor.ActorName;


	}
}
