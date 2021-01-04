using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class InventoryButtonFiller : MonoBehaviour
{

	[SerializeField]
	private LocalizeStringEvent buttonActionText;

	[SerializeField]
	private Button buttonAction;


	public void FillInventoryButtons(ItemType itemType, bool isInteractable = true)
	{



		buttonAction.interactable = isInteractable;

		buttonActionText.StringReference = itemType.ActionName;






	}



}
