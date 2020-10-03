using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GraphicsPresets", menuName = "Graphics/Presets", order = 1)]
public class SettingsPresetsScriptableObject : ScriptableObject
{
    public List<AdvancedGraphics> presetList;
    
    [Serializable]
    public struct AdvancedGraphics
    {
        public string name;
        public ShadowQuality shadowQuality;
        public AnisotropicFiltering anisotropicFiltering;
        public int antiAliasing;
        public float shadowDistance;
        public bool custom;
    }

    public string[] GetPresetNames()
    {
        string[] presetNames = new string[presetList.Count];
        for (int i = 0; i < presetList.Count; i++)
        {
            presetNames[i] = presetList[i].name;
        }

        return presetNames;
    }
}
