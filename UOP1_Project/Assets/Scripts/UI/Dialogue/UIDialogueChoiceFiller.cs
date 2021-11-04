using UnityEngine;
using UnityEngine.Localization.Components;

public class UIDialogueChoiceFiller : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _choiceText = default;
	[SerializeField] private MultiInputButton _actionButton = default;

	[Header("Broadcasting on")]
	[SerializeField] private DialogueChoiceChannelSO _onChoiceMade = default;

	private Choice _currentChoice;

	public void FillChoice(Choice choiceToFill, bool isSelected)
	{
		_currentChoice = choiceToFill;
		_choiceText.StringReference = choiceToFill.Response;
		_actionButton.interactable = true;

		if (isSelected)
		{
			_actionButton.UpdateSelected();
		}
	}

	public void ButtonClicked()
	{
		_onChoiceMade.RaiseEvent(_currentChoice);
	}
}
