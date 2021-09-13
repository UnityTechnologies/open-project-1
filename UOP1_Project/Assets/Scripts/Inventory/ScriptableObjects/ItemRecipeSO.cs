using System.Collections.Generic;
using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

[CreateAssetMenu(fileName = "ItemRecipe", menuName = "Inventory/Recipe")]
public class ItemRecipeSO : ItemSO
{
	[SerializeField] private List<ItemStack> _ingredientsList = new List<ItemStack>();
	[SerializeField] private ItemSO _resultingDish = default;

	public override List<ItemStack> IngredientsList => _ingredientsList;
	public override ItemSO ResultingDish => _resultingDish;
}
