using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CreateAssetMenu(fileName = "ItemType", menuName = "Inventory/ItemType", order = 51)]
public class ItemType : ScriptableObject
{
	[Tooltip("The action associated with the item type")]
	[SerializeField]
	private string _actionName = default;

	[Tooltip("The Item's background color in the UI")]
	[SerializeField] private Color _backgroundColor;

	public string ActionName => _actionName;
	public Color BackgroundColor => _backgroundColor;
}
