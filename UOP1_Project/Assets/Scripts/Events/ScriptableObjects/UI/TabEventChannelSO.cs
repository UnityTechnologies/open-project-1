using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Inventory Tab Event Channel")]
public class TabEventChannelSO : DescriptionBaseSO
{
	public UnityAction<InventoryTabSO> OnEventRaised;

	public void RaiseEvent(InventoryTabSO item)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(item);
	}
}
