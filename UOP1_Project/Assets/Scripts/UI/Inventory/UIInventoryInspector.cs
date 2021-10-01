using UnityEngine;

public class UIInventoryInspector : MonoBehaviour
{
	[SerializeField] private UIInspectorDescription _inspectorDescription = default;
	[SerializeField] private UIInspectorIngredients _recipeIngredients = default;

	public void FillInspector(ItemSO itemToInspect, bool[] availabilityArray = null)
	{
		bool isForCooking = (itemToInspect.ItemType.ActionType == ItemInventoryActionType.Cook);

		_inspectorDescription.FillDescription(itemToInspect);

		if (isForCooking && availabilityArray!= null)
		{
			_recipeIngredients.FillIngredients(itemToInspect.IngredientsList, availabilityArray);
			_recipeIngredients.gameObject.SetActive(true);
		}
		else
			_recipeIngredients.gameObject.SetActive(false);
	}
}
