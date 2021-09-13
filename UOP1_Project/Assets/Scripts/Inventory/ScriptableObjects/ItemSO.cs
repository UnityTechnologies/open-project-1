using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class ItemSO : SerializableScriptableObject
{
	[Tooltip("The name of the item")]
	[SerializeField] private LocalizedString _name = default;

	[Tooltip("A preview image for the item")]
	[SerializeField]
	private Sprite _previewImage = default;

	[Tooltip("A description of the item")]
	[SerializeField]
	private LocalizedString _description = default;

	[Tooltip("A description of the item")]
	[SerializeField]
	private int _healthResorationValue = default;

	[Tooltip("The type of item")]
	[SerializeField]
	private ItemTypeSO _itemType = default;

	[Tooltip("A prefab reference for the model of the item")]
	[SerializeField]
	private GameObject _prefab = default;


	public LocalizedString Name => _name;
	public Sprite PreviewImage => _previewImage;
	public LocalizedString Description => _description;
	public int HealthResorationValue => _healthResorationValue;
	public ItemTypeSO ItemType => _itemType;
	public GameObject Prefab => _prefab;
	public virtual List<ItemStack> IngredientsList { get; }
	public virtual ItemSO ResultingDish { get; }

	public virtual bool IsLocalized { get; }
	public virtual LocalizedSprite LocalizePreviewImage { get; }

}
