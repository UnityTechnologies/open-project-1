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
	[SerializeField] private Button _actionButton = default;

	Choice currentChoice;

	public void FillChoice(Choice choiceToFill)
	{
		currentChoice = choiceToFill;
		_choiceText.StringReference = choiceToFill.Response;
	}

	public void ButtonClicked()
	{
		if (_makeAChoiceEvent != null)
			_makeAChoiceEvent.RaiseEvent(currentChoice);
	}
	public void SetSelected()
	{
		if (_actionButton != null)
			_actionButton.Select();

	}
}
