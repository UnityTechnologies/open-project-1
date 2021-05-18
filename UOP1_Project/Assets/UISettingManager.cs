using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
public class settingTab
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

public class UISettingManager : MonoBehaviour
{
    public List<settingTab> settingTabsList = new List<settingTab>();
    [SerializeField]
    private UISettingTabsFiller _settingTabFiller = default;
    [SerializeField]
    private List<SettingField> _settingFieldsList = default;
    [SerializeField]
    private UISettingFieldsFiller _settingFieldsFiller = default;
    private void Start()
	{
        SetTabs();
        SetFields(SettingTabType.Graphics); 

    }
	public void SetTabs()
	{
        _settingTabFiller.FillTabs(settingTabsList); 
    }

    public void SelectTab(SettingTabType selectedTab)
	{
        _settingTabFiller.SelectTab(selectedTab);
    }

    public void SetFields(SettingTabType selectedTab)
	{
      List<SettingField> fields=  _settingFieldsList.FindAll(o => o.settingTabsType == selectedTab);
        _settingFieldsFiller.FillFields(fields); 


    }
    public void SelectField()
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
   

}
