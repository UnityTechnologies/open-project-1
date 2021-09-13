using System;
using UnityEngine;

[Serializable]
public class ItemStack
{
	[SerializeField] private ItemSO _item;

	public ItemSO Item => _item;

	public int Amount;
	public ItemStack()
	{
		_item = null;
		Amount = 0;
	}
	public ItemStack(ItemStack itemStack)
	{
		_item = itemStack.Item;
		Amount = itemStack.Amount;
	}
	public ItemStack(ItemSO item, int amount)
	{
		_item = item;
		Amount = amount;
	}
}
