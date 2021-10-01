using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UISettingTabFiller : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _localizedTabTitle;
	[SerializeField] private Image _bgSelectedTab;
	[SerializeField] private Color _colorSelectedTab;
	[SerializeField] private Color _colorUnselectedTab;

	SettingsType _currentTabType;

	public UnityAction<SettingsType> Clicked;

	public void SetTab(SettingsType settingTab, bool isSelected)
	{
		_localizedTabTitle.StringReference.TableEntryReference = settingTab.ToString();
		_currentTabType = settingTab;
		if (isSelected)
		{ SelectTab(); }
		else
		{ UnselectTab(); }
	}
	public void SetTab(SettingsType tabType)
	{
		bool isSelected = (_currentTabType == tabType);
		if (isSelected)
		{ SelectTab(); }
		else
		{ UnselectTab(); }
	}
	void SelectTab()
	{
		_bgSelectedTab.enabled = true;
		_localizedTabTitle.GetComponent<TextMeshProUGUI>().color = _colorSelectedTab;

	}
	void UnselectTab()
	{
		_bgSelectedTab.enabled = false;
		_localizedTabTitle.GetComponent<TextMeshProUGUI>().color = _colorUnselectedTab;

	}
	public void Click()
	{
		Clicked.Invoke(_currentTabType);

	}
}
