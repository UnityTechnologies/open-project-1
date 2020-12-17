using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectibleItem : MonoBehaviour
{

	[SerializeField]
	private Item currentItem;

	[SerializeField]
	private SpriteRenderer[] itemImages;
	private void Start()
	{
		SetItem();
	}

	public void PickedItem()
	{


	}

	public Item GetItem()
	{
		Debug.Log("current item " + currentItem);
		return currentItem;

	}

	//this function is only for testing 
	public void SetItem()
	{
		for (int i = 0; i < itemImages.Length; i++)
		{
			itemImages[i].sprite = currentItem.PreviewImage;
		}

	}
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (other.gameObject.GetComponent<ItemPicker>())
			{

				other.gameObject.GetComponent<ItemPicker>().PickItem(currentItem);

			}

		}
	}
}
