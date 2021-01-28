using System;
using UnityEngine;

[Serializable]
public class DropItem
{
	[SerializeField]
	Item _item;

	[SerializeField]
	float _dropRate;

	public Item Item => _item;
	public float DropRate => _dropRate;
}
