using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

public class ItemInstance : MonoBehaviour
{
	[SerializeField] private ItemSO _item = default;

	public ItemSO Item => _item;
}
