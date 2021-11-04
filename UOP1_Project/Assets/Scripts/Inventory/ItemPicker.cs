using UnityEngine;

public class ItemPicker : MonoBehaviour
{
	[Header("Broadcasting on")]
	[SerializeField] ItemEventChannelSO _addItemEvent = default;

	public void PickItem(ItemSO item)
	{
		if (_addItemEvent != null)
			_addItemEvent.RaiseEvent(item);
	}
}
