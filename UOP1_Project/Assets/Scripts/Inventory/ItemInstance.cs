using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
public class ItemInstance : MonoBehaviour
{
	[SerializeField]
	private Item _item = default;

	public Item Item => _item;
}
