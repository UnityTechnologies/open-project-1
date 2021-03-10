using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInspectorFiller : MonoBehaviour
{


	[SerializeField] private InspectorDescriptionFiller _inspectorDescriptionFiller = default;

	[SerializeField] private RecipeIngredientsFiller _recipeIngredientsFiller = default;



	public void FillItemInspector(Item itemToInspect, bool isCookingInventory, bool[] availabilityArray = null)
	{
		_inspectorDescriptionFiller.FillDescription(itemToInspect);

		if (isCookingInventory)
		{
			_recipeIngredientsFiller.FillIngredients(itemToInspect.IngredientsList, availabilityArray);
			_recipeIngredientsFiller.gameObject.SetActive(true);
		}
		else
			_recipeIngredientsFiller.gameObject.SetActive(false);

	}
}
