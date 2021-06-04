using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Serialization;
[System.Serializable]
public enum SettingTabType
{
	Language,
	Audio,
	Graphics

}
[System.Serializable]
public enum SettingFieldType
{
	Language,
	Volume_SFx,
	Volume_Music,
	Resolution,
	FullScreen,
	GraphicQuality,
	AntiAliasing,
	Shadow,

}
[System.Serializable]
public class SettingTab
{
	public SettingTabType settingTabsType;
	public LocalizedString title;
}

[System.Serializable]
public class SettingField
{
	public SettingTabType settingTabsType;
	public SettingFieldType settingFieldType;
	public LocalizedString title;
}

public class UISettings : MonoBehaviour
{
	[SerializeField]
	private List<SettingTab> settingTabsList = new List<SettingTab>();
	[SerializeField]
	private UISettingTabsFiller _settingTabFiller = default;
	[SerializeField]
	private List<SettingField> _settingFieldsList = default;
	[SerializeField]
	private UISettingFieldsFiller _settingFieldsFiller = default;

	public UnityAction Closed;

	[SerializeField]
	private InputReader _inputReader = default;
	private void OnEnable()
	{
		_inputReader.menuCloseEvent += CloseScreen;

	}
	private void OnDisable()
	{
		_inputReader.menuCloseEvent -= CloseScreen;
	}
	public void SetSettingsScreen()
	{

		if (settingTabsList.Count > 0)
		{
			SetTabs();
			SettingTabType defaultTabType = settingTabsList[0].settingTabsType;
			SelectTab(defaultTabType);
		}

	}
	void SetTabs()
	{
		_settingTabFiller.FillTabs(settingTabsList);
	}

	void SelectTab(SettingTabType selectedTab)
	{
		_settingTabFiller.SelectTab(selectedTab);
		SetFields(selectedTab);
	}

	void SetFields(SettingTabType selectedTab)
	{
		List<SettingField> fields = _settingFieldsList.FindAll(o => o.settingTabsType == selectedTab);
		_settingFieldsFiller.FillFields(fields);


	}
	void SelectField()
	{


	}
	public void UnselectField()
	{


	}
	public void NextOption()
	{


	}
	public void PreviousOption()
	{


	}
	public void OpenValidateChoicesPrompt()
	{

	}
	public void ValidateChoices()
	{

	}

	public void CloseScreen()
	{
		Closed.Invoke();
	}


}
