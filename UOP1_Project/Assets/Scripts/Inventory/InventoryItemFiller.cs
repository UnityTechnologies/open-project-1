using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
public class InventoryItemFiller : MonoBehaviour
{
	[SerializeField]

	private Image itemPreviewImage;
	[SerializeField]
	private LocalizeStringEvent itemTitle;
	[SerializeField]
	private TextMeshProUGUI itemCount;
	[SerializeField]
	private Image bgImage;

	[SerializeField]
	private Image imgHover;

	[SerializeField]
	private Image imgSelected;

	public ItemStack currentItem;


	[SerializeField]
	private Button itemButton;

	public void SetItem(ItemStack itemStack, bool isSelected, ItemEventChannelSO selectItemEvent)
	{

		UnhoverItem();

		currentItem = itemStack;

		imgSelected.gameObject.SetActive(isSelected);

		itemPreviewImage.sprite = itemStack.Item.PreviewImage;
		itemTitle.StringReference = itemStack.Item.Name;
		itemCount.text = itemStack.Amount.ToString();
		bgImage.color = itemStack.Item.ItemType.TypeColor;

		itemButton.onClick.RemoveAllListeners();

		itemButton.onClick.AddListener(() =>
		{
			SelectItem();
			UnhoverItem();
			selectItemEvent.RaiseEvent(currentItem.Item);

		});
	}


	public void HoverItem()
	{
		imgHover.gameObject.SetActive(true);


	}
	public void UnhoverItem()
	{

		imgHover.gameObject.SetActive(false);

	}

	public void SelectItem()

	{
		imgSelected.gameObject.SetActive(true);

	}

	public void UnselectItem()
	{


		imgSelected.gameObject.SetActive(false);

	}



}
