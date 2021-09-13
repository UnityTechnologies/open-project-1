using System;
using UnityEngine;

[Serializable]
public class DropItem
{
	[SerializeField] ItemSO _item;
	[SerializeField] float _itemDropRate;

	public ItemSO Item => _item;
	public float ItemDropRate => _itemDropRate;
}
