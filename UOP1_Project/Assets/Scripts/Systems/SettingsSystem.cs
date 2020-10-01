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

    public bool FullScreen { get; private set; }
    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }
    public LanguageSetting Language { get; private set; }
    public AdvancedGraphicsSettings AdvancedGraphics { get; private set; }

    AdvancedGraphicsSettings previousSettings;

    public enum LanguageSetting
    {
        English,
        German,
        //TODO: which languages are going to be supported?
    }

    public struct AdvancedGraphicsSettings
    {
        public ShadowQuality shadowQuality;
        public AnisotropicFiltering anisotropicFiltering;
        public int antiAliasing;
        public float shadowDistance;
        public bool custom;
        public int qualityLevel;
    }

    void Start()
    {
        //TODO: Load previous settings data via save/load interface
        resolutionsDropdown.AddOptions(GetResolutionsDropdownData());
        languageDropdown.AddOptions(GetDropdownData(Enum.GetNames(typeof(LanguageSetting))));
        qualityPresetsDropdown.AddOptions(GetDropdownData(QualitySettings.names, "Custom"));
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
        //TODO: load previous quality level. If custom, set qualityPresetsDropdown to custom. Option "custom" is added in GetQualityPresetsDropdownData()
        AdvancedGraphics = new AdvancedGraphicsSettings()
        {
            anisotropicFiltering = QualitySettings.anisotropicFiltering,
            antiAliasing = QualitySettings.antiAliasing,
            shadowDistance = QualitySettings.shadowDistance,
            shadowQuality = QualitySettings.shadows,
            custom = false,
            qualityLevel = qualityPresetsDropdown.options.Count-1
        };
        previousSettings = AdvancedGraphics;
        qualityPresetsDropdown.SetValueWithoutNotify(AdvancedGraphics.custom ? qualityPresetsDropdown.options.Count-1 : QualitySettings.GetQualityLevel());

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
        anisotropicFilteringDropdown.SetValueWithoutNotify((int) AdvancedGraphics.anisotropicFiltering);
        antiAliasingSlider.SetValueWithoutNotify(AdvancedGraphics.antiAliasing);
        shadowDistanceSlider.SetValueWithoutNotify(AdvancedGraphics.shadowDistance);
        shadowQualityDropdown.SetValueWithoutNotify((int) AdvancedGraphics.shadowQuality);
        shadowDistanceText.text = AdvancedGraphics.shadowDistance.ToString();
        antiAliasingText.text = AdvancedGraphics.antiAliasing.ToString();
        
        qualityPresetsDropdown.value = (AdvancedGraphics.custom ? qualityPresetsDropdown.options.Count-1 : AdvancedGraphics.qualityLevel);
        qualityPresetsDropdown.RefreshShownValue();
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
        AdvancedGraphicsSettings newSettings = AdvancedGraphics;
        newSettings.anisotropicFiltering = (AnisotropicFiltering) anisoLevel;
        newSettings.custom = true;
        AdvancedGraphics = newSettings;
        UpdateAdvancedGraphicsUI();
    }

    public void OnChangeAntialiasing(float value)
    {
        AdvancedGraphicsSettings newSettings = AdvancedGraphics;
        newSettings.antiAliasing = (int)value;
        newSettings.custom = true;
        AdvancedGraphics = newSettings;
        UpdateAdvancedGraphicsUI();
    }

    public void OnChangeShadowDistance(float shadowDistanceValue)
    {
        //TODO: configure min max value in slider
        AdvancedGraphicsSettings newSettings = AdvancedGraphics;
        newSettings.shadowDistance = shadowDistanceValue;
        newSettings.custom = true;
        AdvancedGraphics = newSettings;
        UpdateAdvancedGraphicsUI();
    }

    public void OnChangeShadowQuality(int level)
    {
        AdvancedGraphicsSettings newSettings = AdvancedGraphics;
        newSettings.shadowQuality = (ShadowQuality) level;
        newSettings.custom = true;
        AdvancedGraphics = newSettings;
        UpdateAdvancedGraphicsUI();
    }

    public void OnChangeQualityPreset(int level)
    {
        QualitySettings.SetQualityLevel(level, true);
        Debug.Log("Current quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
        Debug.Log("Antialiasing: " + QualitySettings.antiAliasing);
        Debug.Log("Shadow Distance: " + QualitySettings.shadowDistance);
        AdvancedGraphicsSettings newSettings = AdvancedGraphics;
        newSettings.anisotropicFiltering = QualitySettings.anisotropicFiltering;
        newSettings.antiAliasing = QualitySettings.antiAliasing;
        newSettings.shadowDistance = QualitySettings.shadowDistance;
        newSettings.shadowQuality = QualitySettings.shadows;
        newSettings.qualityLevel = level;
        newSettings.custom = false;
        AdvancedGraphics = newSettings;
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
        if (AdvancedGraphics.custom)
        {
            QualitySettings.anisotropicFiltering = AdvancedGraphics.anisotropicFiltering;
            QualitySettings.antiAliasing = AdvancedGraphics.antiAliasing;
            QualitySettings.shadowDistance = AdvancedGraphics.shadowDistance;
            QualitySettings.shadows = AdvancedGraphics.shadowQuality;
        }
        //No need to save non custom settings as these are applied straight away in OnChangeQualityPreset because there is no way of iterating through unitys quality presets atm
        previousSettings = AdvancedGraphics;
        Debug.Log("Antialiasing: " + QualitySettings.antiAliasing);
    }

    public void OnCancelGraphicsSettings()
    {
        AdvancedGraphics = previousSettings;
        if (AdvancedGraphics.custom)
        {
            QualitySettings.anisotropicFiltering = AdvancedGraphics.anisotropicFiltering;
            QualitySettings.antiAliasing = AdvancedGraphics.antiAliasing;
            QualitySettings.shadowDistance = AdvancedGraphics.shadowDistance;
            QualitySettings.shadows = AdvancedGraphics.shadowQuality;
        }
        else
        {
            QualitySettings.SetQualityLevel(AdvancedGraphics.qualityLevel);
        }
        UpdateAdvancedGraphicsUI();
    }
}