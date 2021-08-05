using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISettingTabsFiller : MonoBehaviour
{
	public UnityAction<SettingsType> ChooseTab;
	[SerializeField]
	private UISettingTabFiller[] _settingTabsList = default;
	public void FillTabs(List<SettingsType> settingTabs)
	{
		for (int i = 0; i < settingTabs.Count; i++)
		{
			_settingTabsList[i].SetTab(settingTabs[i], i == 0);
			_settingTabsList[i].Clicked += ChangeTab;
		}

	}
	private void OnDisable()
	{
		for (int i = 0; i < _settingTabsList.Length; i++)
		{
			_settingTabsList[i].Clicked -= ChangeTab;
		}
	}
	public void SelectTab(SettingsType tabType)
	{
		for (int i = 0; i < _settingTabsList.Length; i++)
		{
			_settingTabsList[i].SetTab(tabType);
		}

	}
	public void ChangeTab(SettingsType tabType)
	{
		ChooseTab.Invoke(tabType);
	}
}
