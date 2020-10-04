using System.Collections.Generic;
using UnityEngine.UI;

namespace Settings.UI
{
    public class GraphicSettingView : DropdownSettingViewBase<GraphicSetting>
    {
        protected override void OnDropdownValueChanged(int index)
        {
            Setting.Value = index;
        }

        protected override List<Dropdown.OptionData> GetOptions()
        {
            var options = new List<Dropdown.OptionData>();
            
            foreach (var graphicLevel in Setting.GetAvailableGraphicLevels())
            {
                options.Add(new Dropdown.OptionData(graphicLevel));
            }

            return options;
        }

        protected override int GetCurrentValue()
        {
            return Setting.Value;
        }
    }
}