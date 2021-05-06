using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using TMPro;
using UnityEngine.Localization;

public class IngredientFiller : MonoBehaviour
{

	[SerializeField]
	private TextMeshProUGUI _ingredientAmount = default;

	[SerializeField]
	private GameObject _availableCheckMark = default;
	[SerializeField]
	private GameObject _unavailableCheckMark = default;

	[SerializeField]
	private GameObject _tooltip = default;

	[SerializeField]
	private LocalizeStringEvent _tooltipMessage = default;

	[SerializeField]
	private Image _ingredientIcon = default;

	[SerializeField]
	private Color _textColorAvailable = default;
	[SerializeField]
	private Color _textColorUnavailable = default;

	public void FillIngredient(ItemStack ingredient, bool isAvailable)
	{
		if (isAvailable)
		{

			_ingredientAmount.color = _textColorAvailable;
		}
		else
		{

			_ingredientAmount.color = _textColorUnavailable;
		}

		_ingredientAmount.text = ingredient.Amount.ToString();
		_tooltipMessage.StringReference = ingredient.Item.Name;
		_tooltipMessage.StringReference.Arguments = new[] { new { Amount = ingredient.Amount } };

		_ingredientIcon.sprite = ingredient.Item.PreviewImage;
		_availableCheckMark.SetActive(isAvailable);
		_unavailableCheckMark.SetActive(!isAvailable);

	}
	public void HoveredItem()
	{
		_tooltip.SetActive(true);
	}
	public void UnHoveredItem()
	{
		_tooltip.SetActive(false);
	}
}
