using System.Collections.Generic;
using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
public class Inventory : ScriptableObject
{
    [Tooltip("The collection of items and their quantities.")]
    [SerializeField]
    private Dictionary<Item, int> _items = new Dictionary<Item, int>();

    public void Add(Item item, int count = 1)
    {
        if (count <= 0)
            return;

        if (!_items.ContainsKey(item))
        {
            _items.Add(item, 0);
        }

        _items[item] += count;
    }

    public void Remove(Item item, int count = 1)
    {
        if (count <= 0)
            return;

        if (!_items.ContainsKey(item))
            return;

        _items[item] -= count;

        if (_items[item] <= 0)
        {
            _items.Remove(item);
        }
    }

    public bool Contains(Item item)
    {
        return _items.ContainsKey(item);
    }

    public int Count(Item item)
    {
        if (!_items.ContainsKey(item))
        {
            return 0;
        }

        return _items[item];
    }
}
