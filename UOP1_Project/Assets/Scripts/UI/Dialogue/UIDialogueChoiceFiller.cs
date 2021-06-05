using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIDialogueChoiceFiller : MonoBehaviour
{

	[SerializeField] private LocalizeStringEvent _choiceText = default;
	[SerializeField] private DialogueChoiceChannelSO _makeAChoiceEvent = default;
	[SerializeField] private MultiInputButton _actionButton = default;

	Choice currentChoice;

	public void FillChoice(Choice choiceToFill, bool isSelected)
	{
		currentChoice = choiceToFill;
		_choiceText.StringReference = choiceToFill.Response;
		_actionButton.interactable = true;
		if (isSelected)
		{
			_actionButton.UpdateSelected();
		}

	}

	public void ButtonClicked()
	{
		_makeAChoiceEvent.RaiseEvent(currentChoice);
	}


}
