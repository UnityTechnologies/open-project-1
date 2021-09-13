using UnityEngine;
using UnityEngine.Localization;
// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

public enum itemInventoryType
{
	Recipe,
	Utensil,
	Ingredient,
	Customisation,
	Dish,
}

public enum ItemInventoryActionType
{
	Cook,
	Use,
	Equip,
	DoNothing
}

[CreateAssetMenu(fileName = "ItemType", menuName = "Inventory/ItemType")]
public class ItemTypeSO : ScriptableObject
{
	[SerializeField] private LocalizedString _actionName = default;
	[Tooltip("The Item's background color in the UI")]
	[SerializeField] private Color _typeColor = default;
	[SerializeField] private itemInventoryType _type = default;
	[SerializeField] private ItemInventoryActionType _actionType = default;
	[Tooltip("The tab type under which the item will be added")]
	[SerializeField] private InventoryTabSO _tabType = default;

	public LocalizedString ActionName => _actionName;
	public Color TypeColor => _typeColor;
	public ItemInventoryActionType ActionType => _actionType;
	public itemInventoryType Type => _type;
	public InventoryTabSO TabType => _tabType;
}
