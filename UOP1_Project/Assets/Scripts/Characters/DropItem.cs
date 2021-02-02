using System;
using UnityEngine;

[Serializable]
public class DropItem
{
	[SerializeField]
	Item _item;

	[SerializeField]
	float _itemDropRate;

	public Item Item => _item;
	public float ItemDropRate => _itemDropRate;
}
