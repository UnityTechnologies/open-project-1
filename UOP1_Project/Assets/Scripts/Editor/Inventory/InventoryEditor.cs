using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CustomEditor(typeof(Inventory))]
[CanEditMultipleObjects]
public class InventoryEditor : Editor
{
	private Inventory _inventory = default;
	private Item _newItem = default;

	private void OnEnable()
	{
		_inventory = target as Inventory;
	}

	public override void OnInspectorGUI()
	{
		// Header
		GUILayout.BeginHorizontal();
		GUILayout.Label("Inventory Items", EditorStyles.boldLabel);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);

		if (_inventory.Items.Count > 0)
		{
			DrawInventory();
		}
		else
		{
			GUILayout.Label("No items in inventory.");
		}

		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Add a New Item", EditorStyles.boldLabel);
		GUILayout.EndHorizontal();
		GUILayout.Space(2f);

		GUILayout.BeginHorizontal();
		_newItem = EditorGUILayout.ObjectField(_newItem, typeof(Item), false) as Item;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (_newItem != null)
		{
			if (GUILayout.Button("Add Item"))
			{
				if (_newItem != null)
				{
					_inventory.Add(_newItem, 1);
					_newItem = null;
				}
			}
		}
		GUILayout.EndHorizontal();
	}

	private void DrawInventory()
	{
		List<KeyValuePair<Item, int>> modifiedItems = new List<KeyValuePair<Item, int>>();
		foreach (KeyValuePair<Item, int> item in _inventory.Items)
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.ObjectField(item.Key, typeof(Item), false);
			int newValue = EditorGUILayout.IntField(item.Value);
			if (newValue != item.Value)
			{
				modifiedItems.Add(new KeyValuePair<Item, int>(item.Key, newValue));
			}
			if (GUILayout.Button("+"))
			{
				modifiedItems.Add(new KeyValuePair<Item, int>(item.Key, newValue + 1));
			}
			if (GUILayout.Button("-"))
			{
				modifiedItems.Add(new KeyValuePair<Item, int>(item.Key, newValue - 1));
			}
			if (GUILayout.Button("Remove"))
			{
				modifiedItems.Add(new KeyValuePair<Item, int>(item.Key, 0));
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(2f);
		}

		if (modifiedItems.Count > 0)
		{
			foreach (KeyValuePair<Item, int> modifiedItem in modifiedItems)
			{
				Item item = modifiedItem.Key;
				int newValue = modifiedItem.Value;
				int currentValue = _inventory.Count(item);

				if (newValue < currentValue)
				{
					_inventory.Remove(item, currentValue - newValue);
				}
				else if (newValue > currentValue)
				{
					_inventory.Add(item, newValue - currentValue);
				}
			}
		}
	}
}
