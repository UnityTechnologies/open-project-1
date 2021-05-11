using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization; 
[System.Serializable]
public enum settingTabType
{
    Language,
    Audio,
    Graphics

}
[System.Serializable]
public enum settingFieldType
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
    public settingTabType settingTabsType;
    public LocalizedString title; 
}

[System.Serializable]
public class settingField
{
    public settingTabType settingTabsType;
    public settingFieldType settingFieldType; 
    public LocalizedString title;
}

public class UISettingManager : MonoBehaviour
{
    public List<settingTab> settingTabsList = new List<settingTab>();
    [SerializeField]
    private UISettingTabsFiller _settingTabFiller = default;
    [SerializeField]
    private List<settingField> _settingFieldsList = default;
    [SerializeField]
    private UISettingFieldsFiller _settingFieldsFiller = default;
    private void Start()
	{
        SetTabs();
        SetFields(settingTabType.Graphics); 

    }
	public void SetTabs()
	{
        _settingTabFiller.FillTabs(settingTabsList); 
    }

    public void SelectTab(settingTabType selectedTab)
	{
        _settingTabFiller.SelectTab(selectedTab);
    }

    public void SetFields(settingTabType selectedTab)
	{
      List<settingField> fields=  _settingFieldsList.FindAll(o => o.settingTabsType == selectedTab);
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
