using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{

	[SerializeField]
	ItemEventChannelSo AddItemEvent;

	public void PickItem(Item item)
	{
		if (AddItemEvent != null)
			AddItemEvent.RaiseEvent(item);
	}
}
