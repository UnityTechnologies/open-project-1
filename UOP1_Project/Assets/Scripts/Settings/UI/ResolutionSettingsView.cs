﻿using System.Collections.Generic;
using UnityEngine.UI;

namespace Settings.UI
{
    public class ResolutionSettingsView : DropdownSettingViewBase<ResolutionSetting>
    {
        protected override void OnDropdownValueChanged(int index)
        {
            var availableResolutions = Setting.GetAvailableResolutions();
            Setting.Value = availableResolutions[index];
        }

        protected override List<Dropdown.OptionData> GetOptions()
        {
            var options = new List<Dropdown.OptionData>();
            
            foreach (var resolutionData in Setting.GetAvailableResolutions())
            {
                options.Add(new Dropdown.OptionData(resolutionData.ToString()));
            }

            return options;
        }

        protected override int GetCurrentValue()
        {
            return Setting.GetCurrentResolutionIndex();
        }
    }
}
