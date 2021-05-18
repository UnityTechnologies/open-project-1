using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using TMPro; 
	public class UISettingTabFiller : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _localizedTabTitle;
	[SerializeField] private Image _bgSelectedTab;
	[SerializeField] private Color _colorSelectedTab;
	[SerializeField] private Color _colorUnselectedTab;

	SettingTabType _currentTabType; 
	public void SetTab(settingTab settingTab, bool isSelected)
	{
		_localizedTabTitle.StringReference = settingTab.title;
		_currentTabType = settingTab.settingTabsType; 
		if (isSelected)
		{ SelectTab();  }
		else
		{ UnselectTab();  }
	}
	public void SetTab(SettingTabType tabType)
	{
		bool isSelected = (_currentTabType == tabType);
		if (isSelected)
		{ SelectTab(); }
		else
		{ UnselectTab(); }
	}
	 void SelectTab()
	{
		_bgSelectedTab.enabled=true;
		_localizedTabTitle.GetComponent<TextMeshProUGUI>().color = _colorSelectedTab;

	}
	 void UnselectTab()
	{
		_bgSelectedTab.enabled = false;
		_localizedTabTitle.GetComponent<TextMeshProUGUI>().color = _colorUnselectedTab; 

	}
}
