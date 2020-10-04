namespace Settings.Core
{
    public interface ISettingsStorage
    {
        bool HasKey(string key);

        string Get(string key);

        void Set(string key, string value);

        void Save();

        void Load();

        void Clear();
    }
}