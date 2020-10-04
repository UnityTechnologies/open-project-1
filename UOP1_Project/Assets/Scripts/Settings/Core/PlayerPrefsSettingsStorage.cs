using UnityEngine;

namespace Settings.Core
{
    public class PlayerPrefsSettingsStorage : ISettingsStorage
    {
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public string Get(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void Set(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }

        public void Load()
        {
            // we don't need to load anything using PlayerPrefs
        }

        public void Clear()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}