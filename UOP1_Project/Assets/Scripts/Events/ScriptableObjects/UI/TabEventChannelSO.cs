using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Inventory Tab Event Channel")]
public class TabEventChannelSO : ScriptableObject
{
	public UnityAction<InventoryTabSO> OnEventRaised;
	public void RaiseEvent(InventoryTabSO item)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(item);
	}
}
