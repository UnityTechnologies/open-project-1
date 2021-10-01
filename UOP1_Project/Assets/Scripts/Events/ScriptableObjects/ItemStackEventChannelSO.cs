using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Item interaction events.
/// Example: Pick up an item passed as paramater
/// </summary>

[CreateAssetMenu(menuName = "Events/UI/Item stack Event Channel")]
public class ItemStackEventChannelSO : DescriptionBaseSO
{
	public UnityAction<ItemStack> OnEventRaised;
	
	public void RaiseEvent(ItemStack item)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(item);
	}
}

