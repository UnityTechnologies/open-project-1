using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Item interaction events.
/// Example: Pick up an item passed as paramater
/// </summary>

[CreateAssetMenu(menuName = "Events/UI/Item Event Channel")]
public class ItemEventChannelSO : DescriptionBaseSO
{
	public UnityAction<ItemSO> OnEventRaised;
	
	public void RaiseEvent(ItemSO item)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(item);
	}
}

