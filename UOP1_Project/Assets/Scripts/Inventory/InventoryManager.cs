using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

	[SerializeField] private InventorySO _currentInventory = default;
	[SerializeField] private ItemEventChannelSO _cookRecipeEvent = default;
	[SerializeField] private ItemEventChannelSO _useItemEvent = default;
	[SerializeField] private ItemEventChannelSO _equipItemEvent = default;
	[SerializeField] private ItemStackEventChannelSO _rewardItemEvent = default;
	[SerializeField] private ItemEventChannelSO _giveItemEvent = default;
	[SerializeField] ItemEventChannelSO _addItemEvent = default;
	[SerializeField] ItemEventChannelSO _removeItemEvent = default;
	[SerializeField] private SaveSystem _saveSystem;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors

		_cookRecipeEvent.OnEventRaised += CookRecipeEventRaised;

		_useItemEvent.OnEventRaised += UseItemEventRaised;

		_equipItemEvent.OnEventRaised += EquipItemEventRaised;

		_addItemEvent.OnEventRaised += AddItem;

		_removeItemEvent.OnEventRaised += RemoveItem;

		_rewardItemEvent.OnEventRaised += AddItemStack;

		_giveItemEvent.OnEventRaised += RemoveItem;

	}

	private void OnDisable()
	{

		_cookRecipeEvent.OnEventRaised -= CookRecipeEventRaised;

		_useItemEvent.OnEventRaised -= UseItemEventRaised;

		_equipItemEvent.OnEventRaised -= EquipItemEventRaised;

		_addItemEvent.OnEventRaised -= AddItem;

		_removeItemEvent.OnEventRaised -= RemoveItem;

	}





	void AddItemWithUIUpdate(ItemSO item)
	{

		_currentInventory.Add(item);
		if (_currentInventory.Contains(item))
		{
			ItemStack itemToUpdate = _currentInventory.Items.Find(o => o.Item == item);
			//	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, false);
		}
	}

	void RemoveItemWithUIUpdate(ItemSO item)
	{
		ItemStack itemToUpdate = new ItemStack();

		if (_currentInventory.Contains(item))
		{
			itemToUpdate = _currentInventory.Items.Find(o => o.Item == item);
		}

		_currentInventory.Remove(item);

		bool removeItem = _currentInventory.Contains(item);
		//	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, removeItem);

	}
	void AddItem(ItemSO item)
	{
		_currentInventory.Add(item);
		_saveSystem.SaveDataToDisk();

	}
	void AddItemStack(ItemStack itemStack)
	{
		_currentInventory.Add(itemStack.Item, itemStack.Amount);
		_saveSystem.SaveDataToDisk();

	}
	void RemoveItem(ItemSO item)
	{
		_currentInventory.Remove(item);
		_saveSystem.SaveDataToDisk();
	}


	void CookRecipeEventRaised(ItemSO recipe)
	{

		//find recipe
		if (_currentInventory.Contains(recipe))
		{
			List<ItemStack> ingredients = recipe.IngredientsList;
			//remove ingredients (when it's a consumable)
			if (_currentInventory.hasIngredients(ingredients))
			{
				for (int i = 0; i < ingredients.Count; i++)
				{
					if ((ingredients[i].Item.ItemType.ActionType == ItemInventoryActionType.Use))
						_currentInventory.Remove(ingredients[i].Item, ingredients[i].Amount);
				}
				//add dish
				_currentInventory.Add(recipe.ResultingDish);
			}

		}

		_saveSystem.SaveDataToDisk();



	}

	public void UseItemEventRaised(ItemSO item)
	{
		RemoveItem(item);
	}

	public void EquipItemEventRaised(ItemSO item)
	{

	}
}

