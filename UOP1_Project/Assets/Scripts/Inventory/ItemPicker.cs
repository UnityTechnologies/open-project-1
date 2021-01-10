using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
	[SerializeField] private ItemEventChannelSo _addItemEvent;

	public void PickItem(Item item)
	{
		if (_addItemEvent != null)
			_addItemEvent.RaiseEvent(item);
	}
}
