using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;

public class UIDialogueChoiceFiller : MonoBehaviour
{
	[FormerlySerializedAs("choiceText")]
	[SerializeField] private LocalizeStringEvent _choiceText;
	[FormerlySerializedAs("MakeAChoiceEvent")]
	[SerializeField] private DialogueChoiceChannelSO _makeAChoiceEvent;

	Choice currentChoice;

	public void FillChoice(Choice choiceToFill)
	{
		currentChoice = choiceToFill;
		_choiceText.StringReference = choiceToFill.Response.Sentence;
	}

	public void ButtonClicked()
	{
		if(_makeAChoiceEvent!=null)
		_makeAChoiceEvent.RaiseEvent(currentChoice); 
	}
}
