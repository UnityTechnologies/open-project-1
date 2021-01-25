using System;
using UnityEngine;

[Serializable]
public class DropItem
{
	[SerializeField]
	Item _item;

	[SerializeField]
	float _itemDropRate;

	[SerializeField]
	private GameObject _collectibleItemPrefab;

	public Item Item => _item;
	public float ItemDropRate => _itemDropRate;
	public GameObject CollectibleItemPrefab => _collectibleItemPrefab;
}
