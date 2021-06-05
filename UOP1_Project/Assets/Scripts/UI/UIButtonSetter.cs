using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIButtonSetter : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _buttonText = default;
	[SerializeField] private MultiInputButton _button = default;

	public UnityAction Clicked = default;

	private bool isDefaultSelection = false;


	private void OnDisable()
	{
		_button.IsSelected = false;
		isDefaultSelection = false;
	}
	public void SetButton(bool isSelect)
	{
		isDefaultSelection = isSelect;
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
