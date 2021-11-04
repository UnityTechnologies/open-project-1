using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Events;

public class UISettingItemFiller : MonoBehaviour
{
	[SerializeField] private SettingFieldType _fieldType = default;
	[SerializeField] private UIPaginationFiller _pagination = default;
	[SerializeField] private LocalizeStringEvent _currentSelectedOption_LocalizedEvent = default;
	[SerializeField] private LocalizeStringEvent _title = default;
	[SerializeField] private TextMeshProUGUI _currentSelectedOption_Text = default;
	[SerializeField] private Image _bg = default;
	[SerializeField] private Color _colorSelected = default;
	[SerializeField] private Color _colorUnselected = default;
	[SerializeField] private Sprite _bgSelected = default;
	[SerializeField] private Sprite _bgUnselected = default;
	[SerializeField] private MultiInputButton _buttonNext = default;
	[SerializeField] private MultiInputButton _buttonPrevious = default;

	public event UnityAction OnNextOption = delegate { };
	public event UnityAction OnPreviousOption = delegate { };

	public void FillSettingField_Localized(int paginationCount, int selectedPaginationIndex, string selectedOption)
	{
		_pagination.SetPagination(paginationCount, selectedPaginationIndex);
		_title.StringReference.TableEntryReference = _fieldType.ToString(); // Set title following the Field Type. Field type is the Table Reference
		_currentSelectedOption_LocalizedEvent.StringReference.TableEntryReference = _fieldType + "_" + selectedOption;

		_currentSelectedOption_LocalizedEvent.enabled = true;

		_buttonNext.interactable = (selectedPaginationIndex < paginationCount - 1);
		_buttonPrevious.interactable = (selectedPaginationIndex > 0);
	}
	
	public void FillSettingField(int paginationCount, int selectedPaginationIndex, string selectedOption_int)
	{
		_pagination.SetPagination(paginationCount, selectedPaginationIndex);
		_title.StringReference.TableEntryReference = _fieldType.ToString(); // Set title following the Field Type. Field type is the Table Reference
		_currentSelectedOption_LocalizedEvent.enabled = false;
		_currentSelectedOption_Text.text = selectedOption_int.ToString();

		_buttonNext.interactable = (selectedPaginationIndex < paginationCount - 1);
		_buttonPrevious.interactable = (selectedPaginationIndex > 0);
	}

	public void SelectItem()
	{
		_bg.sprite = _bgSelected;
		_title.GetComponent<TextMeshProUGUI>().color = _colorSelected;
		_currentSelectedOption_Text.color = _colorSelected;
	}

	public void UnselectItem()
	{
		_bg.sprite = _bgUnselected;

		_title.GetComponent<TextMeshProUGUI>().color = _colorUnselected;
		_currentSelectedOption_Text.color = _colorUnselected;
	}

	public void NextOption()
	{
		OnNextOption.Invoke();
	}

	public void PreviousOption()
	{
		OnPreviousOption.Invoke();
	}

	public void SetNavigation(MultiInputButton buttonToSelectOnDown, MultiInputButton buttonToSelectOnUp)
	{
		MultiInputButton[] buttonNavigation = GetComponentsInChildren<MultiInputButton>();
		foreach (MultiInputButton button in buttonNavigation)
		{
			Navigation newNavigation = new Navigation();
			newNavigation.mode = Navigation.Mode.Explicit;
			if (buttonToSelectOnDown != null)
				newNavigation.selectOnDown = buttonToSelectOnDown;
			if (buttonToSelectOnDown != null)
				newNavigation.selectOnUp = buttonToSelectOnUp;
			newNavigation.selectOnLeft = button.navigation.selectOnLeft;
			newNavigation.selectOnRight = button.navigation.selectOnRight;
			button.navigation = newNavigation;
		}
	}
}
