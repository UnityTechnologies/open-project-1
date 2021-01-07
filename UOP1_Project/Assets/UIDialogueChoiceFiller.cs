using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components; 
public class UIDialogueChoiceFiller : MonoBehaviour
{
    [SerializeField]
	LocalizeStringEvent choiceText;
	[SerializeField]
	DialogueChoiceChannelSO MakeAChoiceEvent;

	Choice currentChoice;

	public void FillChoice(Choice choiceToFill)
	{
		currentChoice = choiceToFill;
		choiceText.StringReference = choiceToFill.Response.Sentence;
	}

	public void ButtonClicked()
	{
		if(MakeAChoiceEvent!=null)
		MakeAChoiceEvent.RaiseEvent(currentChoice); 
	}
}
