using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField]
	private Inventory currentInventory;

	[SerializeField]
	private ItemEvent CookRecipeEvent;
	[SerializeField]
	private ItemEvent UseItemEvent;
	[SerializeField]
	private ItemEvent EquipItemEvent;



	[SerializeField]
	ItemEvent AddItemEvent;
	[SerializeField]
	ItemEvent RemoveItemEvent;

	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		if (CookRecipeEvent != null)
		{
			CookRecipeEvent.eventRaised += CookRecipeEventRaised;
		}
		if (UseItemEvent != null)
		{
			UseItemEvent.eventRaised += UseItemEventRaised;
		}
		if (EquipItemEvent != null)
		{
			EquipItemEvent.eventRaised += EquipItemEventRaised;
		}
		if (AddItemEvent != null)
		{
			AddItemEvent.eventRaised += AddItem;
		}
		if (RemoveItemEvent != null)
		{
			RemoveItemEvent.eventRaised += RemoveItem;
		}
	}

	private void OnDisable()
	{
		if (CookRecipeEvent != null)
		{
			CookRecipeEvent.eventRaised -= CookRecipeEventRaised;
		}
		if (UseItemEvent != null)
		{
			UseItemEvent.eventRaised -= UseItemEventRaised;
		}
		if (EquipItemEvent != null)
		{
			EquipItemEvent.eventRaised -= EquipItemEventRaised;
		}
		if (AddItemEvent != null)
		{
			AddItemEvent.eventRaised -= AddItem;
		}
		if (RemoveItemEvent != null)
		{
			RemoveItemEvent.eventRaised -= RemoveItem;
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
					if (ingredients[i].Item.ItemType.ActionType == ItemInventoryActionType.use)
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

