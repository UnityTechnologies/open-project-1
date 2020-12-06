using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;
public class IngredientFiller : MonoBehaviour
{

	[SerializeField]
	private TextMeshProUGUI ingredientAmount;
	[SerializeField]
	private LocalizeStringEvent ingredientName;
	[SerializeField]
	private GameObject checkMark;

	public void FillIngredient(ItemStack ingredient, bool isAvailable)
	{

		ingredientAmount.text = ingredient.Amount.ToString();
		ingredientName.StringReference = ingredient.Item.Name;
		checkMark.SetActive(isAvailable);

	}
}
