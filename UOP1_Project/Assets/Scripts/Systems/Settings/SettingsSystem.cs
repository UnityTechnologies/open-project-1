using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsSystem : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionsDropdown;
    [SerializeField] TMP_Dropdown languageDropdown;
    [SerializeField] TMP_Dropdown shadowQualityDropdown;
    [SerializeField] TMP_Dropdown anisotropicFilteringDropdown;
    [SerializeField] TMP_Dropdown qualityPresetsDropdown;
    [SerializeField] Slider antiAliasingSlider;
    [SerializeField] Slider shadowDistanceSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] TextMeshProUGUI antiAliasingText;
    [SerializeField] TextMeshProUGUI shadowDistanceText;
    [SerializeField] SettingsPresetsScriptableObject settingsPresets;

    public bool FullScreen { get; private set; }
    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }
    public LanguageSetting Language { get; private set; }

    SettingsPresetsScriptableObject.AdvancedGraphics currentAdvancedGraphics, previousAdvancedGraphics;
    int currentQualityLevel, previousQualityLevel; //-1 is custom

    public enum LanguageSetting
    {
        English,
        German,
        //TODO: which languages are going to be supported?
    }

    void Start()
    {
        //TODO: Load previous settings data via save/load interface
        resolutionsDropdown.AddOptions(GetResolutionsDropdownData());
        languageDropdown.AddOptions(GetDropdownData(Enum.GetNames(typeof(LanguageSetting))));
        qualityPresetsDropdown.AddOptions(GetDropdownData(settingsPresets.GetPresetNames(), "Custom"));
        foreach (string s in QualitySettings.names)
        {
            Debug.Log(s);
        }
        anisotropicFilteringDropdown.AddOptions(GetDropdownData(Enum.GetNames(typeof(AnisotropicFiltering))));
        shadowQualityDropdown.AddOptions(GetDropdownData(Enum.GetNames(typeof(ShadowQuality))));
        LoadCurrentSettings();
    }

    void LoadCurrentSettings()
    {
        int resolutionIndex = 0;
        //TODO: load previous resolution setting. If not existing find current resolution index
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.currentResolution.ToString() == Screen.resolutions[i].ToString())
            {
                resolutionIndex = i;
            }
        }

        resolutionsDropdown.SetValueWithoutNotify(resolutionIndex);
        //TODO: load quality level from previous session. If custom, set qualityPresetsDropdown to custom. Option "custom" is added in GetQualityPresetsDropdownData()
        previousQualityLevel = currentQualityLevel;
        currentAdvancedGraphics = settingsPresets.presetList[currentQualityLevel]; //Set to lowest preset initially
        previousAdvancedGraphics = currentAdvancedGraphics;
        qualityPresetsDropdown.SetValueWithoutNotify(currentAdvancedGraphics.custom ? qualityPresetsDropdown.options.Count-1 : currentQualityLevel);

        UpdateAdvancedGraphicsUI();

        //TODO: load previous language setting
        Language = LanguageSetting.English;
        languageDropdown.SetValueWithoutNotify((int) Language);
        //TODO: load previous fullscreen setting
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        //TODO: load previous music volume setting
        MusicVolume = 0.5f;
        musicVolumeSlider.SetValueWithoutNotify(MusicVolume);
        //TODO: load previous sfx volume setting
        SfxVolume = 0.5f;
        sfxVolumeSlider.SetValueWithoutNotify(SfxVolume);
    }

    void SelectGraphicsPreset(int level)
    {
        QualitySettings.SetQualityLevel(0, true);
        UpdateAdvancedGraphicsUI();
    }

    void UpdateAdvancedGraphicsUI()
    {
        anisotropicFilteringDropdown.SetValueWithoutNotify((int) currentAdvancedGraphics.anisotropicFiltering);
        antiAliasingSlider.SetValueWithoutNotify(currentAdvancedGraphics.antiAliasing);
        shadowDistanceSlider.SetValueWithoutNotify(currentAdvancedGraphics.shadowDistance);
        shadowQualityDropdown.SetValueWithoutNotify((int) currentAdvancedGraphics.shadowQuality);
        shadowDistanceText.text = currentAdvancedGraphics.shadowDistance.ToString();
        antiAliasingText.text = currentAdvancedGraphics.antiAliasing.ToString();
        
        qualityPresetsDropdown.SetValueWithoutNotify(currentAdvancedGraphics.custom ? qualityPresetsDropdown.options.Count-1 : currentQualityLevel);
    }

    List<TMP_Dropdown.OptionData> GetResolutionsDropdownData()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            options.Add(new TMP_Dropdown.OptionData(Screen.resolutions[i].ToString()));
        }

        return options;
    }

    List<TMP_Dropdown.OptionData> GetDropdownData(string[] optionNames, params string[] customOptions)
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (string option in optionNames)
        {
            options.Add(new TMP_Dropdown.OptionData(option));
        }

        foreach (string option in customOptions)
        {
            options.Add(new TMP_Dropdown.OptionData(option));
        }
        return options;
    }

    #region GENERAL SETTINGS

    public void OnChangeLanguage(int languageIndex)
    {
        Language = (LanguageSetting) languageIndex;
        Debug.Log("Language set to: " + Language);
    }

    #endregion

    #region GRAPHICS SETTINGS

    public void OnChangeFullscreen(bool fullScreen)
    {
        Screen.fullScreenMode = fullScreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
    }

    public void OnChangeResolution(int resolutionIndex)
    {
        Resolution newResolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreenMode);
    }

    public void OnChangeAnisotropicFiltering(int anisoLevel)
    {
        currentAdvancedGraphics.anisotropicFiltering = (AnisotropicFiltering) anisoLevel;
        currentAdvancedGraphics.custom = true;
        UpdateAdvancedGraphicsUI();
    }

    public void OnChangeAntialiasing(float value)
    {
        currentAdvancedGraphics.antiAliasing = (int) value;
        currentAdvancedGraphics.custom = true;
        UpdateAdvancedGraphicsUI();
    }

    public void OnChangeShadowDistance(float shadowDistanceValue)
    {
        //TODO: configure min max value in slider
        currentAdvancedGraphics.shadowDistance = shadowDistanceValue;
        currentAdvancedGraphics.custom = true;
        UpdateAdvancedGraphicsUI();
    }

    public void OnChangeShadowQuality(int level)
    {
        currentAdvancedGraphics.shadowQuality = (ShadowQuality) level;
        currentAdvancedGraphics.custom = true;
        UpdateAdvancedGraphicsUI();
    }

    public void OnChangeQualityPreset(int level)
    {
        if (level >= settingsPresets.presetList.Count)
        {
            //Custom level chosen
            currentAdvancedGraphics.custom = true;
        }
        else
        {
            currentAdvancedGraphics = settingsPresets.presetList[level];
        }
        currentQualityLevel = level;
        UpdateAdvancedGraphicsUI();
    }

    #endregion

    #region AUDIO SETTINGS

    //TODO: clamp volume to [0, 1] or [0, 100]? Change Slider min max value in editor depending on use case
    public void OnChangeMusicVolume(float volume)
    {
        MusicVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
    }

    //TODO: clamp volume to [0, 1] or [0, 100]? Change Slider min max value in editor depending on use case
    public void OnChangeSfxVolume(float volume)
    {
        SfxVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
    }

    #endregion

    public void OnSaveGraphicsSettings()
    {
        QualitySettings.anisotropicFiltering = currentAdvancedGraphics.anisotropicFiltering;
        QualitySettings.antiAliasing = currentAdvancedGraphics.antiAliasing;
        QualitySettings.shadowDistance = currentAdvancedGraphics.shadowDistance;
        QualitySettings.shadows = currentAdvancedGraphics.shadowQuality;
        
        previousAdvancedGraphics = currentAdvancedGraphics;
        previousQualityLevel = currentQualityLevel;
        Debug.Log("Antialiasing: " + QualitySettings.antiAliasing);
        Debug.Log("Anisotropic Filtering: " + QualitySettings.anisotropicFiltering);
        Debug.Log("Shadow Distance: " + QualitySettings.shadowDistance);
        Debug.Log("Shadow Quality: " + QualitySettings.shadows);
    }

    public void OnCancelGraphicsSettings()
    {
        currentAdvancedGraphics = previousAdvancedGraphics;
        currentQualityLevel = previousQualityLevel;
        QualitySettings.anisotropicFiltering = currentAdvancedGraphics.anisotropicFiltering;
        QualitySettings.antiAliasing = currentAdvancedGraphics.antiAliasing;
        QualitySettings.shadowDistance = currentAdvancedGraphics.shadowDistance;
        QualitySettings.shadows = currentAdvancedGraphics.shadowQuality;
        
        UpdateAdvancedGraphicsUI();
    }
}