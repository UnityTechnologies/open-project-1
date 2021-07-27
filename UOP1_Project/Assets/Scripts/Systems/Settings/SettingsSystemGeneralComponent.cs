using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsSystemGeneralComponent : SettingsSystemSubComponents
{
    [SerializeField] TMP_Dropdown languageDropdown;

    SettingsSystem settingsSystem;
    public LanguageSetting Language { get; private set; }
    
    public enum LanguageSetting
    {
        English,
        German,
        //TODO: which languages are going to be supported?
    }

    void Start()
    {
        Setup();
    }

    protected override void Setup()
    {
        settingsSystem = transform.parent.GetComponent<SettingsSystem>();
        languageDropdown.AddOptions(GetDropdownData(Enum.GetNames(typeof(LanguageSetting))));
        //TODO: Load previous serialized session data via Load/Save class
        Language = SettingsSystemGeneralComponent.LanguageSetting.English;
        languageDropdown.SetValueWithoutNotify((int) Language);
    }

    #region UI CALLBACKS
    
    public void OnChangeLanguage(int languageIndex)
    {
        Language = (LanguageSetting) languageIndex;
        Debug.Log("Language set to: " + Language);
    }
    
    #endregion
}
