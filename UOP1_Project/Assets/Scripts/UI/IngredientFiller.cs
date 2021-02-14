using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;

public class IngredientFiller : MonoBehaviour
{

	[SerializeField]
	private TextMeshProUGUI _ingredientAmount = default;

	[SerializeField]
	private LocalizeStringEvent _ingredientName = default;

	[SerializeField]
	private GameObject _checkMark = default;

	public void FillIngredient(ItemStack ingredient, bool isAvailable)
	{

		_ingredientAmount.text = ingredient.Amount.ToString();
		_ingredientName.StringReference = ingredient.Item.Name;
		_checkMark.SetActive(isAvailable);

	}
}
