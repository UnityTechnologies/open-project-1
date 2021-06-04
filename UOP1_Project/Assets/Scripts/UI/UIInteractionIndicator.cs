using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;
using UnityEngine.UI;

public class UIInteractionIndicator : MonoBehaviour
{
	[SerializeField] LocalizeStringEvent _interactionName = default;

	[SerializeField]
	UIButtonPrompt buttonPromptSetter = default;

	public void FillInteractionPanel(InteractionSO interactionItem)
	{
		_interactionName.StringReference = interactionItem.InteractionName;
		bool isKeyboard = true;
		//	bool isKeyboard = !(Input.GetJoystickNames() != null && Input.GetJoystickNames().Length > 0);
		buttonPromptSetter.SetButtonPrompt(isKeyboard);

	}


}
