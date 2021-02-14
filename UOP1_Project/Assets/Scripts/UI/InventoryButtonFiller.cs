using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class InventoryButtonFiller : MonoBehaviour
{

	[SerializeField]
	private LocalizeStringEvent _buttonActionText = default;

	[SerializeField]
	private Button _buttonAction = default;


	public void FillInventoryButtons(ItemType itemType, bool isInteractable = true)
	{



		_buttonAction.interactable = isInteractable;

		_buttonActionText.StringReference = itemType.ActionName;






	}



}
