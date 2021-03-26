using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Inventory Tab Event Channel")]
public class TabEventChannelSO : ScriptableObject
{
	public UnityAction<InventoryTabType> OnEventRaised;
	public void RaiseEvent(InventoryTabType item)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(item);
	}
}
