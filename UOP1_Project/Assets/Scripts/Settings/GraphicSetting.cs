using Settings.Core;
using UnityEngine;

namespace Settings
{
    public class GraphicSetting : SettingBase<int>
    {
        public override int Value
        {
            get => QualitySettings.GetQualityLevel();
            set => QualitySettings.SetQualityLevel(value, true); //TODO: leave this 'true' or not?
        }

        public override void Load(string saveData)
        {
            if (!int.TryParse(saveData, out int value))
            {
                SetDefault();
                return;
            }

            Value = value;
        }

        public override string Save()
        {
            return Value.ToString();
        }

        public string[] GetAvailableGraphicLevels()
        {
            return QualitySettings.names;
        }
    }
}