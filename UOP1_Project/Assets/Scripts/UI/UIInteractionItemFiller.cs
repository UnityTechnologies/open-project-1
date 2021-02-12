using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;

public class UIInteractionItemFiller : MonoBehaviour
{
	[SerializeField] LocalizeStringEvent _interactionName = default;

	[SerializeField] TextMeshProUGUI _interactionKeyButton = default;


	public void FillInteractionPanel(InteractionSO interactionItem)
	{
		_interactionName.StringReference = interactionItem.InteractionName;
		_interactionKeyButton.text = KeyCode.E.ToString(); // this keycode will be modified later on 

	}
}
