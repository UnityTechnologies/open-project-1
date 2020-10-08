using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Settings.UI
{
    public class LanguageSettingView : DropdownSettingViewBase<LanguageSetting>
    {
        protected override void OnDropdownValueChanged(int index)
        {
            string[] availableLanguages = Setting.GetAvailableLanguages();
            Setting.Value = availableLanguages[index];
        }

        protected override List<Dropdown.OptionData> GetOptions()
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            
            foreach (string language in Setting.GetAvailableLanguages())
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