using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorFiller : MonoBehaviour
{

	[SerializeField] private InventoryInspectorFiller _inventoryInspector = default;

	public void FillItemInspector(Item itemToInspect, bool[] availabilityArray = null)
	{

		bool isForCooking = (itemToInspect.ItemType.ActionType == ItemInventoryActionType.cook);

		_inventoryInspector.gameObject.SetActive(true);
		_inventoryInspector.FillItemInspector(itemToInspect, isForCooking, availabilityArray);


	}

	public void HideItemInspector()
	{
		_inventoryInspector.gameObject.SetActive(false);


	}

}
