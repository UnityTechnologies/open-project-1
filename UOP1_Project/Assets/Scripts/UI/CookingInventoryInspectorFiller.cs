using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingInventoryInspectorFiller : MonoBehaviour
{

	[SerializeField] private InspectorPreviewFiller _inspectorPreviewFiller = default;

	[SerializeField] private InspectorDescriptionFiller _inspectorDescriptionFiller = default;

	[SerializeField] private RecipeIngredientsFiller _recipeIngredientsFiller = default;



	public void FillItemInspector(Item itemToInspect, bool[] availabilityArray)
	{

		_inspectorPreviewFiller.FillPreview(itemToInspect);
		_inspectorDescriptionFiller.FillDescription(itemToInspect);
		_recipeIngredientsFiller.FillIngredients(itemToInspect.IngredientsList, availabilityArray);

	}
}
