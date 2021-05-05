using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

	[SerializeField] private Inventory _currentInventory = default;
	[SerializeField] private ItemEventChannelSO _cookRecipeEvent = default;
	[SerializeField] private ItemEventChannelSO _useItemEvent = default;
	[SerializeField] private ItemEventChannelSO _equipItemEvent = default;
	[SerializeField] private ItemEventChannelSO _rewardItemEvent = default;
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
		
			_rewardItemEvent.OnEventRaised += AddItem;
		
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





	void AddItemWithUIUpdate(Item item)
	{

		_currentInventory.Add(item);
		if (_currentInventory.Contains(item))
		{
			ItemStack itemToUpdate = _currentInventory.Items.Find(o => o.Item == item);
			//	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, false);
		}
	}

	void RemoveItemWithUIUpdate(Item item)
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
	void AddItem(Item item)
	{
		_currentInventory.Add(item);
		_saveSystem.SaveDataToDisk();

	}
	void RemoveItem(Item item)
	{
		_currentInventory.Remove(item);
		_saveSystem.SaveDataToDisk();
	}


	void CookRecipeEventRaised(Item recipe)
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
					if ((ingredients[i].Item.ItemType.ActionType == ItemInventoryActionType.use))
						_currentInventory.Remove(ingredients[i].Item, ingredients[i].Amount);
				}
				//add dish
				_currentInventory.Add(recipe.ResultingDish);
			}

		}




	}

	public void UseItemEventRaised(Item item)
	{
		RemoveItem(item);
	}

	public void EquipItemEventRaised(Item item)
	{

	}
}

