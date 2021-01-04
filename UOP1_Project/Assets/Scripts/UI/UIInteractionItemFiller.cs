using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;

public class UIInteractionItemFiller : MonoBehaviour
{

	[SerializeField]
	LocalizeStringEvent interactionName;

	[SerializeField]
	TextMeshProUGUI interactionKeyButton;


	public void FillInteractionPanel(InteractionSO interactionItem)
	{
		interactionName.StringReference = interactionItem.InteractionName;
		interactionKeyButton.text = KeyCode.E.ToString(); // this keycode will be modified later on 

	}
}
