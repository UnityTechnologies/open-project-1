using UnityEngine;
using UnityEngine.Localization;
// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

public enum InventoryTabType
{
	Customization,
	CookingItem,
	Recipe
}

[CreateAssetMenu(fileName = "InventoryTabType", menuName = "Inventory/Inventory Tab Type")]
public class InventoryTabSO : ScriptableObject
{
	[SerializeField] private Sprite _tabIcon = default;
	[SerializeField] private InventoryTabType _tabType = default;

	public Sprite TabIcon => _tabIcon;
	public InventoryTabType TabType => _tabType;
}
