using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class SettingsSystemSubComponents : MonoBehaviour
{
    protected List<TMP_Dropdown.OptionData> GetDropdownData(string[] optionNames, params string[] customOptions)
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

    protected abstract void Setup();
}
