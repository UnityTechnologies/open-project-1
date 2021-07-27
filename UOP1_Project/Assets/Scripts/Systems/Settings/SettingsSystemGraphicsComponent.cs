using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSystemGraphicsComponent : SettingsSystemSubComponents
{
    [SerializeField] TMP_Dropdown resolutionsDropdown;
    [SerializeField] TMP_Dropdown shadowQualityDropdown;
    [SerializeField] TMP_Dropdown anisotropicFilteringDropdown;
    [SerializeField] TMP_Dropdown graphicsPresetsDropdown;
    [SerializeField] Slider antiAliasingSlider;
    [SerializeField] Slider shadowDistanceSlider;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] TextMeshProUGUI antiAliasingText;
    [SerializeField] TextMeshProUGUI shadowDistanceText;
    [SerializeField] SettingsPresetsScriptableObject settingsPresets;
    
    bool setupComplete;

    //Advanced graphic settings
    SettingsPresetsScriptableObject.AdvancedGraphics currentAdvancedGraphics, previousAdvancedGraphics;
    int currentQualityIndex, previousQualityIndex;

    //Fullscreen settings
    public bool FullScreen { get; private set; }
    bool previouslyInFullScreen;
    
    //Resolution setting
    Resolution currentResolution, previousResolution;

    void OnEnable()
    {
        if (!setupComplete)
        {
            Setup();
        }
    }
    
    protected override void Setup()
    {
        resolutionsDropdown.AddOptions(GetResolutionsDropdownData());
        graphicsPresetsDropdown.AddOptions(GetDropdownData(Enum.GetNames(typeof(SettingsPresetsScriptableObject.GraphicsQualityLevel)), "Custom"));
        anisotropicFilteringDropdown.AddOptions(GetDropdownData(Enum.GetNames(typeof(AnisotropicFiltering))));
        shadowQualityDropdown.AddOptions(GetDropdownData(Enum.GetNames(typeof(ShadowQuality))));
        
        //RESOLUTION
        //TODO: apparently, unity keeps the resolution which was set in the last session. But there is no method to get this resolution in the next session
        //TODO: because Screen.currentResolution in windowed mode gives back the resolution of the desktop, not the unity window resolution
        currentResolution = Screen.resolutions[Screen.resolutions.Length - 1]; //Initially set highest possible resolution on device
        previousResolution = currentResolution;
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreenMode, currentResolution.refreshRate);
        SelectCurrentResolution();
        
        //FULLSCREEN MODE
        //TODO: load previous session fullscreen setting
        FullScreen = Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen;
        previouslyInFullScreen = FullScreen;
        fullscreenToggle.SetIsOnWithoutNotify(FullScreen);
        
        //QUALITY PRESET
        //TODO: instead of hardcoding the level here, load data from previous session
        currentQualityIndex = (int) SettingsPresetsScriptableObject.GraphicsQualityLevel.Low;
        previousQualityIndex = currentQualityIndex;
        currentAdvancedGraphics = settingsPresets.presetList[currentQualityIndex]; //Set to lowest preset initially
        previousAdvancedGraphics = currentAdvancedGraphics;
        graphicsPresetsDropdown.SetValueWithoutNotify(currentAdvancedGraphics.custom ? graphicsPresetsDropdown.options.Count-1 : currentQualityIndex);
        UpdateUI();
        setupComplete = true;
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
    
    void UpdateUI()
    {
        anisotropicFilteringDropdown.SetValueWithoutNotify((int) currentAdvancedGraphics.anisotropicFiltering);
        antiAliasingSlider.SetValueWithoutNotify(currentAdvancedGraphics.antiAliasing);
        shadowDistanceSlider.SetValueWithoutNotify(currentAdvancedGraphics.shadowDistance);
        shadowQualityDropdown.SetValueWithoutNotify((int) currentAdvancedGraphics.shadowQuality);
        shadowDistanceText.text = currentAdvancedGraphics.shadowDistance.ToString();
        antiAliasingText.text = currentAdvancedGraphics.antiAliasing.ToString();
        fullscreenToggle.SetIsOnWithoutNotify(FullScreen);
        graphicsPresetsDropdown.SetValueWithoutNotify(currentAdvancedGraphics.custom ? graphicsPresetsDropdown.options.Count-1 : currentQualityIndex);
    }

    #region UI CALLBACKS

    public void OnChangeFullscreen(bool fullScreen)
    {
        Screen.fullScreenMode = fullScreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        FullScreen = fullScreen;
    }

    public void OnChangeResolution(int resolutionIndex)
    {
        Debug.Log("Index: "  + resolutionIndex);
        currentResolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreenMode, currentResolution.refreshRate);
    }

    public void OnChangeAnisotropicFiltering(int anisoLevel)
    {
        currentAdvancedGraphics.anisotropicFiltering = (AnisotropicFiltering) anisoLevel;
        currentAdvancedGraphics.custom = true;
        UpdateUI();
    }

    public void OnChangeAntialiasing(float value)
    {
        currentAdvancedGraphics.antiAliasing = (int) value;
        currentAdvancedGraphics.custom = true;
        UpdateUI();
    }

    public void OnChangeShadowDistance(float shadowDistanceValue)
    {
        //TODO: configure min max value in slider
        currentAdvancedGraphics.shadowDistance = shadowDistanceValue;
        currentAdvancedGraphics.custom = true;
        UpdateUI();
    }

    public void OnChangeShadowQuality(int level)
    {
        currentAdvancedGraphics.shadowQuality = (ShadowQuality) level;
        currentAdvancedGraphics.custom = true;
        UpdateUI();
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
        currentQualityIndex = level;
        UpdateUI();
    }

    #endregion

    void SelectCurrentResolution()
    {
        int resolutionIndex = 0;
        //TODO: is resolution persistent once it is set? Need to test in build
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (currentResolution.width == Screen.resolutions[i].width &&
                currentResolution.height == Screen.resolutions[i].height &&
                currentResolution.refreshRate == Screen.resolutions[i].refreshRate)
            {
                resolutionIndex = i;
            }
        }
        
        resolutionsDropdown.SetValueWithoutNotify(resolutionIndex);
    }
    
    public void OnSaveGraphicsSettings()
    {
        QualitySettings.anisotropicFiltering = currentAdvancedGraphics.anisotropicFiltering;
        QualitySettings.antiAliasing = currentAdvancedGraphics.antiAliasing;
        QualitySettings.shadowDistance = currentAdvancedGraphics.shadowDistance;
        QualitySettings.shadows = currentAdvancedGraphics.shadowQuality;
        
        previousAdvancedGraphics = currentAdvancedGraphics;
        previousQualityIndex = currentQualityIndex;
        previousResolution = currentResolution;
        previouslyInFullScreen = FullScreen;
        Debug.Log("Saving resolution state: " + currentResolution);
        Debug.Log(("Saving fullscreen mode state: " + previouslyInFullScreen));
    }

    public void OnCancelGraphicsSettings()
    {
        currentAdvancedGraphics = previousAdvancedGraphics;
        currentQualityIndex = previousQualityIndex;
        QualitySettings.anisotropicFiltering = currentAdvancedGraphics.anisotropicFiltering;
        QualitySettings.antiAliasing = currentAdvancedGraphics.antiAliasing;
        QualitySettings.shadowDistance = currentAdvancedGraphics.shadowDistance;
        QualitySettings.shadows = currentAdvancedGraphics.shadowQuality;
        Screen.SetResolution(previousResolution.width, previousResolution.height, previouslyInFullScreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed, previousResolution.refreshRate);
        // It might be a good idea to wait for the end of frame here with a coroutine, because setResolution actually is executed at the end of the next frame
        //TODO: Observe this for issues in future
        currentResolution = previousResolution;
        FullScreen = previouslyInFullScreen;
        SelectCurrentResolution();
        UpdateUI();
        Debug.Log("New Resolution: " + previousResolution);
        Debug.Log("New fullscreen mode: " + FullScreen);
    }
}
