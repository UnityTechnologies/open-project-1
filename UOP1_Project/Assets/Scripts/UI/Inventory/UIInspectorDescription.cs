using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;

public class UIInspectorDescription : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _textDescription = default;
	[SerializeField] private TextMeshProUGUI _textHealthRestoration = default;
	[SerializeField] private LocalizeStringEvent _textName = default;

	public void FillDescription(ItemSO itemToInspect)
	{
		_textName.StringReference = itemToInspect.Name;
		_textName.StringReference.Arguments = new[] { new { Purpose = 0, Amount = 1 } };
		_textDescription.StringReference = itemToInspect.Description;
		if (itemToInspect.HealthResorationValue > 0)
		{
			_textHealthRestoration.text = "+" + itemToInspect.HealthResorationValue;
		}
		else
		{
			_textHealthRestoration.text = "";

		}
		_textName.gameObject.SetActive(true);
		_textDescription.gameObject.SetActive(true);
	}
}