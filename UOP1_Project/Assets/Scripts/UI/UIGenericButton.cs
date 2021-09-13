using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class UIGenericButton : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _buttonText = default;
	[SerializeField] private MultiInputButton _button = default;

	public UnityAction Clicked = default;

	private bool _isDefaultSelection = false;

	private void OnDisable()
	{
		_button.IsSelected = false;
		_isDefaultSelection = false;
	}

	public void SetButton(bool isSelect)
	{
		_isDefaultSelection = isSelect;
		if (isSelect)
			_button.UpdateSelected();
	}

	public void SetButton(LocalizedString localizedString, bool isSelected)
	{
		_buttonText.StringReference = localizedString;

		if (isSelected)
			SelectButton();
	}

	public void SetButton(string tableEntryReference, bool isSelected)
	{
		_buttonText.StringReference.TableEntryReference = tableEntryReference;

		if (isSelected)
			SelectButton();
	}

	void SelectButton()
	{
		_button.Select();
	}

	public void Click()
	{
		Clicked.Invoke();
	}
}
