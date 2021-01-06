using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
public class UIDialogueManager : MonoBehaviour
{
	[SerializeField] LocalizeStringEvent lineText = default;

	[SerializeField] LocalizeStringEvent actorNameText = default;
	public void SetDialogue(DialogueLineSO dialogueLine, ActorSO actor)
	{

		lineText.StringReference = dialogueLine.Sentence;
		actorNameText.StringReference = actor.ActorName; 


	}
}
