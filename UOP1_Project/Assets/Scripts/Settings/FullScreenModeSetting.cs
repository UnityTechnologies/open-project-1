using System;
using Settings.Core;
using UnityEngine;

namespace Settings
{
    public class FullScreenModeSetting : SettingBase<FullScreenMode>
    {
        public override FullScreenMode Value
        {
            get => Screen.fullScreenMode;
            set => Screen.fullScreenMode = value;
        }

        public override void Load(string saveData)
        {
            if (!Enum.TryParse(saveData, out FullScreenMode value))
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
    }
}