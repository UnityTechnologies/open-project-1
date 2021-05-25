using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingTabsFiller : MonoBehaviour
{
    [SerializeField]
    private UISettingTabFiller [] _settingTabsList = default;
    public void FillTabs(List<settingTab> settingTabs)
	{
		for (int i = 0; i < settingTabs.Count; i++)
		{
			_settingTabsList[i].SetTab(settingTabs[i], i == 0);
		}

	}
	public void SelectTab(SettingTabType tabType)
	{
		for (int i = 0; i < _settingTabsList.Length; i++)
		{
			_settingTabsList[i].SetTab(tabType);
		}

	}
}
