using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using TMPro;

public class IngredientFiller : MonoBehaviour
{

	[SerializeField]
	private TextMeshProUGUI _ingredientAmount = default;

	[SerializeField]
	private GameObject _checkMark = default;

	[SerializeField]
	private Image _ingredientIcon = default;

	public void FillIngredient(ItemStack ingredient, bool isAvailable)
	{

		_ingredientAmount.text = ingredient.Amount.ToString();
		_ingredientAmount.gameObject.SetActive(isAvailable);
		_ingredientIcon.sprite = ingredient.Item.PreviewImage; 
		_checkMark.SetActive(!isAvailable);

	}
}
