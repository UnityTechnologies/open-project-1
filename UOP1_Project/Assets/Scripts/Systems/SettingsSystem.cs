using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SettingsSystem : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionsDropdown;
    
    public bool FullScreen { get; private set; }

    void Awake()
    {
        EnsureGameObjectIntegrity();
        resolutionsDropdown.AddOptions(GetDropdownResolutionData());
    }

    void EnsureGameObjectIntegrity()
    {
        if (resolutionsDropdown == null)
        {
            resolutionsDropdown = transform.Find("Resolutions Dropdown").GetComponent<TMP_Dropdown>();
        }
    }
    
    List<TMP_Dropdown.OptionData> GetDropdownResolutionData()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            options.Add(new TMP_Dropdown.OptionData(Screen.resolutions[i].ToString()));
        }

        return options;
    }

    public void OnChangeFullscreen(bool fullScreen)
    {
        Screen.fullScreenMode = fullScreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
    }

    public void OnChangeResolution(int resolutionIndex)
    {
        Resolution newResolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreenMode);
    }
}
