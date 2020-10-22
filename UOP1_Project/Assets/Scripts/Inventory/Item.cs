using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
public abstract class Item : ScriptableObject
{
	[Tooltip("The Item's name")]
	[SerializeField] private string _itemName; // TODO: Consider using a string table for localization
	[Tooltip("A preview Image for the Item")]
	[SerializeField] private Sprite _itemImage;
	[Tooltip("A prefab reference for the model of the item.")]
	[SerializeField] private MeshRenderer _itemModel; // use this as reference  prefab
	[Tooltip("The Item's description")]
	[SerializeField] [Multiline] private string _itemDescription; // TODO: Consider using a string table for localization
	[SerializeField] private string _itemActionName; // TODO: Consider using a string table for localization
	[Tooltip("The Item's background color in the UI")]
	[SerializeField] private Color _itemBackgroundColor;

	// TODO: Add common functionality for properties
}
