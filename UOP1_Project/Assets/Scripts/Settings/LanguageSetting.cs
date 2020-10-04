using Settings.Core;

namespace Settings
{
    public class LanguageSetting : SettingBase<string>
    {
        public override void Load(string saveData)
        {
            Value = saveData;
        }

        public override string Save()
        {
            return Value;
        }

        // todo
        public string[] GetAvailableLanguages()
        {
            return new[]
            {
                "English",
            };
        }

        public override void SetDefault()
        {
            base.SetDefault();
            Value = "English"; // todo: make use of System.Globalization.CultureInfo.CurrentCulture.Name
        }
    }
}