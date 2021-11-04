using System.Collections.Generic;
using UnityEngine;

public class UIInspectorIngredients : MonoBehaviour
{
	[SerializeField] private List<UIInspectorIngredientFiller> _instantiatedGameObjects = new List<UIInspectorIngredientFiller>();

	public void FillIngredients(List<ItemStack> listofIngredients, bool[] availabilityArray)
	{

		int maxCount = Mathf.Max(listofIngredients.Count, _instantiatedGameObjects.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < listofIngredients.Count)
			{
				if (i >= _instantiatedGameObjects.Count)
				{
					//Do nothing, maximum ingredients for a recipe reached
					Debug.Log("Maximum ingredients reached");
				}
				else
				{
					bool isAvailable = availabilityArray[i];
					_instantiatedGameObjects[i].FillIngredient(listofIngredients[i], isAvailable);

					_instantiatedGameObjects[i].gameObject.SetActive(true);
				}
			}
			else if (i < _instantiatedGameObjects.Count)
			{
				_instantiatedGameObjects[i].gameObject.SetActive(false);
			}
		}
	}
}