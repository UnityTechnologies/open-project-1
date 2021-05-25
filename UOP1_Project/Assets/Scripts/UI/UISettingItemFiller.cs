using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization; 
using UnityEngine.Events;

public class UISettingItemFiller : MonoBehaviour
{
	[SerializeField] private UIPaginationFiller _pagination = default;
	[SerializeField] private TextMeshProUGUI _currentSelectedOption = default;


	[SerializeField] private Image _bg = default;
	[SerializeField] private LocalizeStringEvent _title = default;


	[SerializeField] private Color _colorSelected = default;
	[SerializeField] private Color _colorUnselected = default;

	[SerializeField] private Sprite _bgSelected = default;
	[SerializeField] private Sprite _bgUnselected = default;

	 private SettingFieldType _fieldType = default;


	public event UnityAction _nextOption = delegate { };
	public event UnityAction _previousOption = delegate { };
	
	public void SetSettingField(int paginationCount, int selectedPaginationIndex, string selectedOption, LocalizedString fieldTitle, SettingFieldType fieldType)
	{
		_fieldType = fieldType; 
		_pagination.SetPagination(paginationCount, selectedPaginationIndex);
		_currentSelectedOption.text = selectedOption;
		_title.StringReference= fieldTitle; 
	}
	public void SetSettingNewOption(int selectedPaginationIndex, string selectedOption)
	{
		_pagination.SetCurrentPagination(selectedPaginationIndex);
		_currentSelectedOption.text = selectedOption;
	}

	public void SelectItem()
	{
		_bg.sprite = _bgSelected;
		_title.GetComponent<TextMeshProUGUI>().color = _colorSelected;
		_currentSelectedOption.color = _colorSelected;
	}
	public void UnselectItem()
	{
		_bg.sprite = _bgUnselected;
		_title.GetComponent<TextMeshProUGUI>().color = _colorUnselected;
		_currentSelectedOption.color = _colorUnselected;

	}

	public void NextOption()
	{
		_nextOption.Invoke(); 

	}
	public void PreviousOption()
	{
		_previousOption.Invoke(); 
	}
	


}
