using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] GameObject generalSettings, graphicsSettings, audioSettings;
    [SerializeField] Button generalSettingsButton, graphicsSettingsButton, audioSettingsButton, saveGraphicsSettingsButton, cancelGraphicsSettingsButton;

    public enum SettingsType
    {
        General,
        Graphics,
        Audio
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

    public void OnSaveGraphicsSettings()
    {
        saveGraphicsSettingsButton.interactable = false;
    }

    void OpenSetting(SettingsType settingType)
    {
        generalSettings.SetActive(settingType == SettingsType.General);
        graphicsSettings.SetActive((settingType == SettingsType.Graphics));
        audioSettings.SetActive(settingType == SettingsType.Audio);
        
        generalSettingsButton.interactable = settingType != SettingsType.General;
        graphicsSettingsButton.interactable = settingType != SettingsType.Graphics;
        audioSettingsButton.interactable = settingType != SettingsType.Audio;
    }
}
