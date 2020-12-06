using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingInventoryInspectorFiller : MonoBehaviour
{
	[SerializeField]
	private InspectorPreviewFiller inspectorPreviewFiller;

	[SerializeField]
	private InspectorDescriptionFiller inspectorDescriptionFiller;

	[SerializeField]
	private RecipeIngredientsFiller recipeIngredientsFiller;



	public void FillItemInspector(Item itemToInspect, bool[] availabilityArray)
	{

		inspectorPreviewFiller.FillPreview(itemToInspect);
		inspectorDescriptionFiller.FillDescription(itemToInspect);
		recipeIngredientsFiller.FillIngredients(itemToInspect.IngredientsList, availabilityArray);

	}
}
