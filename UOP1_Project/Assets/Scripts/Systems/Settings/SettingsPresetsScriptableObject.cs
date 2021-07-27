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
        public GraphicsQualityLevel qualityLevel;
        public ShadowQuality shadowQuality;
        public AnisotropicFiltering anisotropicFiltering;
        public int antiAliasing;
        public float shadowDistance;
        public bool custom;
    }

    public enum GraphicsQualityLevel
    {
        Low,
        Middle,
        High
    }

    public AdvancedGraphics GetPresetByQualityLevel(GraphicsQualityLevel level)
    {
        foreach (AdvancedGraphics preset in presetList)
        {
            if (level == preset.qualityLevel)
            {
                return preset;
            }
        }

        return default;
    }
}
