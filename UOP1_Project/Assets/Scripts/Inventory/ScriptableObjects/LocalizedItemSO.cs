using UnityEngine;
using UnityEngine.Localization;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

[CreateAssetMenu(fileName = "LocalizedItem", menuName = "Inventory/Localized Item")]
public class LocalizedItemSO : ItemSO
{
	[SerializeField] private bool _isLocalized = false; 
	[SerializeField] private LocalizedSprite _localizePreviewImage = default;

	public override bool IsLocalized => _isLocalized;
	public override LocalizedSprite LocalizePreviewImage => _localizePreviewImage;
}
