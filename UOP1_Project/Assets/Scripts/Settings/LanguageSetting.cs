using Settings.Core;

namespace Settings
{
    public class LanguageSetting : SettingBase<string>
    {
        public override string Value
        {
            set
            {
                base.Value = value;
                //TODO: set language for LocalizationSystem.
            }
        }

        public override void Load(string saveData)
        {
            Value = saveData;
        }

        public override string Save()
        {
            return Value;
        }

        //TODO: specify array of supported languages from inspector or from chosen LocalizationSystem. 
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
            Value = "English"; //TODO: make use of System.Globalization.CultureInfo.CurrentCulture.Name
        }
    }
}