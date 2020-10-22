using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
public abstract class Item : ScriptableObject
{
	[Tooltip("The Item's name")]
	[SerializeField] private string _name; // TODO: Consider using a string table for localization
	[Tooltip("A preview Image for the Item")]
	[SerializeField] private Sprite _image;
	[Tooltip("A prefab reference for the model of the item.")]
	[SerializeField] private MeshRenderer _model; // use this as reference prefab
	[Tooltip("The Item's description")]
	[SerializeField] [Multiline] private string _description; // TODO: Consider using a string table for localization
	[SerializeField] private string _actionName; // TODO: Consider using a string table for localization
	[Tooltip("The Item's background color in the UI")]
	[SerializeField] private Color _backgroundColor;

	// TODO: Add common functionality for properties
}
