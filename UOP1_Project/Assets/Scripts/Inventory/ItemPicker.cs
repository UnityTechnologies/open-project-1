using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
	[SerializeField] ItemEventChannelSO _addItemEvent = default;

	public void PickItem(ItemSO item)
	{
		Debug.Log("Pick Item");
		if (_addItemEvent != null)
			_addItemEvent.RaiseEvent(item);
	}
}
