using System.Collections.Generic;
using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory", order = 51)]
public class Inventory : ScriptableObject
{
	[Tooltip("The collection of items and their quantities.")]
	[SerializeField]
	private List<ItemStack> _items = new List<ItemStack>();

	public List<ItemStack> Items => _items;

	public void Add(Item item, int count = 1)
	{
		if (count <= 0)
			return;

		for (int i = 0; i < _items.Count; i++)
		{
			ItemStack currentItemStack = _items[i];
			if (item == currentItemStack.Item)
			{
				//only add to the amount if the item is usable 
				if (currentItemStack.Item.ItemType.ActionType == ItemInventoryActionType.use)
				{
					currentItemStack.Amount += count;
				}

				return;
			}
		}

		_items.Add(new ItemStack(item, count));
	}

	public void Remove(Item item, int count = 1)
	{
		if (count <= 0)
			return;

		for (int i = 0; i < _items.Count; i++)
		{
			ItemStack currentItemStack = _items[i];

			if (currentItemStack.Item == item)
			{
				currentItemStack.Amount -= count;

				if (currentItemStack.Amount <= 0)
					_items.Remove(currentItemStack);

				return;
			}
		}
	}

	public bool Contains(Item item)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (item == _items[i].Item)
			{
				return true;
			}
		}

		return false;
	}

	public int Count(Item item)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			ItemStack currentItemStack = _items[i];
			if (item == currentItemStack.Item)
			{
				return currentItemStack.Amount;
			}
		}

		return 0;
	}

	public bool[] IngredientsAvailability(List<ItemStack> ingredients)
	{

		bool[] availabilityArray = new bool[ingredients.Count];

		for (int i = 0; i < ingredients.Count; i++)
		{
			availabilityArray[i] = _items.Exists(o => o.Item == ingredients[i].Item && o.Amount >= ingredients[i].Amount);

		}
		return availabilityArray;


	}
	public bool hasIngredients(List<ItemStack> ingredients)
	{

		bool hasIngredients = !ingredients.Exists(j => !_items.Exists(o => o.Item == j.Item && o.Amount >= j.Amount));

		return hasIngredients;


	}
}
