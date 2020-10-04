using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Settings.UI
{
    public class LanguageSettingView : DropdownSettingViewBase<LanguageSetting>
    {
        protected override void OnDropdownValueChanged(int index)
        {
            var availableLanguages = Setting.GetAvailableLanguages();
            Setting.Value = availableLanguages[index];
        }

        protected override List<Dropdown.OptionData> GetOptions()
        {
            var options = new List<Dropdown.OptionData>();
            
            foreach (var language in Setting.GetAvailableLanguages())
            {
                options.Add(new Dropdown.OptionData(language));
            }

            return options;
        }

        protected override int GetCurrentValue()
        {
            return Array.FindIndex(Setting.GetAvailableLanguages(), s => s == Setting.Value);
        }
    }
}