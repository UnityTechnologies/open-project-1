using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] GameObject generalSettings, graphicsSettings, audioSettings, inputSettings;
    [SerializeField] Button generalSettingsButton, graphicsSettingsButton, audioSettingsButton, inputSettingsButton, saveGraphicsSettingsButton, cancelGraphicsSettingsButton;

    public enum SettingsType
    {
        General,
        Graphics,
        Audio,
        Input
    }

    public void OpenGeneralSettings()
    {
        OpenSetting(SettingsType.General);
    }

    public void OpenGraphicsSettings()
    {
        OpenSetting(SettingsType.Graphics);
    }

    public void OpenAudioSettings()
    {
        OpenSetting(SettingsType.Audio);
    }

    public void OpenInputSettings()
    {
        OpenSetting(SettingsType.Input);
    }

    public void OnSaveGraphicsSettings()
    {
        saveGraphicsSettingsButton.interactable = false;
    }

    void OpenSetting(SettingsType settingType)
    {
        generalSettings.SetActive(settingType == SettingsType.General);
        graphicsSettings.SetActive((settingType == SettingsType.Graphics));
        audioSettings.SetActive(settingType == SettingsType.Audio);
        inputSettings.SetActive(settingType == SettingsType.Input);
        
        generalSettingsButton.interactable = settingType != SettingsType.General;
        graphicsSettingsButton.interactable = settingType != SettingsType.Graphics;
        audioSettingsButton.interactable = settingType != SettingsType.Audio;
        inputSettingsButton.interactable = settingType != SettingsType.Input;
    }
}
