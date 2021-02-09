using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField]
	private Inventory currentInventory;

	[SerializeField]
	private ItemEventChannelSO CookRecipeEvent;
	[SerializeField]
	private ItemEventChannelSO UseItemEvent;
	[SerializeField]
	private ItemEventChannelSO EquipItemEvent;



	[SerializeField]
	ItemEventChannelSO AddItemEvent;
	[SerializeField]
	ItemEventChannelSO RemoveItemEvent;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (CookRecipeEvent != null)
		{
			CookRecipeEvent.OnEventRaised += CookRecipeEventRaised;
		}
		if (UseItemEvent != null)
		{
			UseItemEvent.OnEventRaised += UseItemEventRaised;
		}
		if (EquipItemEvent != null)
		{
			EquipItemEvent.OnEventRaised += EquipItemEventRaised;
		}
		if (AddItemEvent != null)
		{
			AddItemEvent.OnEventRaised += AddItem;
		}
		if (RemoveItemEvent != null)
		{
			RemoveItemEvent.OnEventRaised += RemoveItem;
		}
	}

	private void OnDisable()
	{
		if (CookRecipeEvent != null)
		{
			CookRecipeEvent.OnEventRaised -= CookRecipeEventRaised;
		}
		if (UseItemEvent != null)
		{
			UseItemEvent.OnEventRaised -= UseItemEventRaised;
		}
		if (EquipItemEvent != null)
		{
			EquipItemEvent.OnEventRaised -= EquipItemEventRaised;
		}
		if (AddItemEvent != null)
		{
			AddItemEvent.OnEventRaised -= AddItem;
		}
		if (RemoveItemEvent != null)
		{
			RemoveItemEvent.OnEventRaised -= RemoveItem;
		}
	}





	void AddItemWithUIUpdate(Item item)
	{

		currentInventory.Add(item);
		if (currentInventory.Contains(item))
		{
			ItemStack itemToUpdate = currentInventory.Items.Find(o => o.Item == item);
			//	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, false);
		}
	}

	void RemoveItemWithUIUpdate(Item item)
	{
		ItemStack itemToUpdate = new ItemStack();

		if (currentInventory.Contains(item))
		{
			itemToUpdate = currentInventory.Items.Find(o => o.Item == item);
		}

		currentInventory.Remove(item);

		bool removeItem = currentInventory.Contains(item);
		//	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, removeItem);

	}
	void AddItem(Item item)
	{
		currentInventory.Add(item);

	}
	void RemoveItem(Item item)
	{
		currentInventory.Remove(item);

	}


	void CookRecipeEventRaised(Item recipe)
	{

		//find recipe
		if (currentInventory.Contains(recipe))
		{
			List<ItemStack> ingredients = recipe.IngredientsList;
			//remove ingredients (when it's a consumable)
			if (currentInventory.hasIngredients(ingredients))
			{
				for (int i = 0; i < ingredients.Count; i++)
				{
					if ((ingredients[i].Item.ItemType.ActionType == ItemInventoryActionType.use))
						currentInventory.Remove(ingredients[i].Item, ingredients[i].Amount);
				}
				//add dish
				currentInventory.Add(recipe.ResultingDish);
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

