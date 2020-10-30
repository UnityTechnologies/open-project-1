using System;
using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 51)]
[Serializable]
public class Item : ScriptableObject
{
	[Tooltip("The name of the item")]
	public string Name;

	[Tooltip("A preview image for the item")]
	[SerializeField]
	public Sprite PreviewImage;

	[Tooltip("A description of the item")]
	[SerializeField]
	[Multiline]
	public string Description;

	[Tooltip("The type of item")]
	[SerializeField]
	public ItemType ItemType;

	[Tooltip("A prefab reference for the model of the item")]
	[SerializeField]
	public GameObject Prefab;
}
