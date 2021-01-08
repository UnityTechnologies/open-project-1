using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorFiller : MonoBehaviour
{

	[SerializeField]
	private SimpleInventoryInspectorFiller simpleInventoryInspector;

	[SerializeField]
	private CookingInventoryInspectorFiller cookingInventoryInspector;

	public void FillItemInspector(Item itemToInspect, bool[] availabilityArray = null)
	{
		bool isForCooking = (itemToInspect.ItemType.ActionType == ItemInventoryActionType.cook);

		simpleInventoryInspector.gameObject.SetActive(!isForCooking);
		cookingInventoryInspector.gameObject.SetActive(isForCooking);

		if (!isForCooking)
		{

			simpleInventoryInspector.FillItemInspector(itemToInspect);
		}
		else
		{
			cookingInventoryInspector.FillItemInspector(itemToInspect, availabilityArray);
		}


	}

	public void HideItemInspector()
	{
		simpleInventoryInspector.gameObject.SetActive(false);
		cookingInventoryInspector.gameObject.SetActive(false);


	}

}
