using System.Collections.Generic;
using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory", order = 51)]
public class Inventory : ScriptableObject
{
	[Tooltip("The collection of items and their quantities.")]
	[SerializeField]
	private Dictionary<Item, int> items = new Dictionary<Item, int>();

	public void Add(Item item, int count = 1)
	{
		if (count <= 0)
			return;

		if (!items.ContainsKey(item))
		{
			items.Add(item, 0);
		}

		items[item] += count;
	}

	public void Remove(Item item, int count = 1)
	{
		if (count <= 0)
			return;

		if (!items.ContainsKey(item))
			return;

		items[item] -= count;

		if (items[item] <= 0)
		{
			items.Remove(item);
		}
	}

	public bool Contains(Item item)
	{
		return items.ContainsKey(item);
	}

	public int Count(Item item)
	{
		if (!items.ContainsKey(item))
		{
			return 0;
		}

		return items[item];
	}
}
