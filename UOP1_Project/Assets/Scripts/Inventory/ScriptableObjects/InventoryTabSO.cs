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
[CreateAssetMenu(fileName = "InventoryTabType", menuName = "Inventory/Inventory Tab Type", order = 51)]
public class InventoryTabSO : ScriptableObject
{
	[Tooltip("The tab Name that will be displayed in the inventory")]
	[SerializeField]
	private LocalizedString _tabName = default;

	[Tooltip("The tab Picture that will be displayed in the inventory")]
	[SerializeField]
	private Sprite _tabIcon = default;


	[Tooltip("The tab type used to reference the item")]
	[SerializeField] private InventoryTabType _tabType = default;

	public LocalizedString TabName => _tabName;
	public Sprite TabIcon => _tabIcon;
	public InventoryTabType TabType => _tabType;
}
