using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 51)]
public class Item : ScriptableObject
{
	[Tooltip("The name of the item")]
	[SerializeField]
	private string _name = default;

	[Tooltip("A preview image for the item")]
	[SerializeField]
	private Sprite _previewImage = default;

	[Tooltip("A description of the item")]
	[SerializeField]
	[Multiline]
	private string _description = default;


	[Tooltip("The type of item")]
	[SerializeField]
	private ItemType _itemType = default;

	[Tooltip("A prefab reference for the model of the item")]
	[SerializeField]
	private GameObject _prefab = default;

	public string Name => _name;
	public Sprite PreviewImage => _previewImage;
	public string Description => _description;
	public ItemType ItemType => _itemType;
	public GameObject Prefab => _prefab;

}
