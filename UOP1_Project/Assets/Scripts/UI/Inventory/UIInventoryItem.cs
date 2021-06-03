using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Events;

public class UIInventoryItem : MonoBehaviour
{
	[SerializeField] private Image _itemPreviewImage = default;
	[SerializeField] private TextMeshProUGUI _itemCount = default;
	[SerializeField] private Image _bgImage = default;
	[SerializeField] private Image _imgHover = default;
	[SerializeField] private Image _imgSelected = default;
	[HideInInspector] public ItemStack _currentItem = default;
	[SerializeField] private Button _itemButton = default;
	[SerializeField] private Image _bgInactiveImage = default;

	public UnityAction<Item> ItemSelected;

	[SerializeField] private LocalizeSpriteEvent _bgLocalizedImage = default;

	public void SetItem(ItemStack itemStack, bool isSelected)
	{
		_itemPreviewImage.gameObject.SetActive(true);
		_itemCount.gameObject.SetActive(true);
		_bgImage.gameObject.SetActive(true);
		_imgHover.gameObject.SetActive(true);
		_imgSelected.gameObject.SetActive(true);
		_itemButton.gameObject.SetActive(true);
		_bgInactiveImage.gameObject.SetActive(false);

		UnhoverItem();
		_currentItem = itemStack;

		_imgSelected.gameObject.SetActive(isSelected);

		if (itemStack.Item.IsLocalized)
		{
			_bgLocalizedImage.enabled = true;
			_bgLocalizedImage.AssetReference = itemStack.Item.LocalizePreviewImage;
		}
		else
		{
			_bgLocalizedImage.enabled = false;
			_itemPreviewImage.sprite = itemStack.Item.PreviewImage;
		}
		_itemCount.text = itemStack.Amount.ToString();
		_bgImage.color = itemStack.Item.ItemType.TypeColor;


	}
	public void SetInactiveItem()
	{
		UnhoverItem();
		_currentItem = null;
		_itemPreviewImage.gameObject.SetActive(false);
		_itemCount.gameObject.SetActive(false);
		_bgImage.gameObject.SetActive(false);
		_imgHover.gameObject.SetActive(false);
		_imgSelected.gameObject.SetActive(false);
		_itemButton.gameObject.SetActive(false);
		_bgInactiveImage.gameObject.SetActive(true);

	}

	public void SelectFirstElement()
	{
		_itemButton.Select();
		SelectItem();
	}

	public void HoverItem()
	{
		_imgHover.gameObject.SetActive(true);
	}
	public void UnhoverItem()
	{

		_imgHover.gameObject.SetActive(false);

	}

	public void SelectItem()
	{
		if (ItemSelected != null && _currentItem != null && _currentItem.Item != null)

		{
			_imgSelected.gameObject.SetActive(true);
			ItemSelected.Invoke(_currentItem.Item);
		}
		else
		{
			_imgSelected.gameObject.SetActive(false);
		}

	}

	public void UnselectItem()
	{
		_imgSelected.gameObject.SetActive(false);

	}



}
