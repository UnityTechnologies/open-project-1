using System.Linq;
using Settings.Controls;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace Settings.Core
{
    // This class derives from ScriptableObject so we can set references using inspector.
    // While this approach works fine, I don't really like it. For now there is no DI or something like this, so...
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings asset")]
    public class GameSettings : ScriptableObject
    {
        #region Temporary solution

        // injecting references through Unity's serialization (inspector)
        [SerializeField] private InputActionAsset inputActionAsset = default;
        [SerializeField] private AudioMixer audioMixer = default;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            Instance = Resources.Load<GameSettings>("GameSettings");
            Instance.InitializeInstance();
            Instance.Load();
            Debug.Log("GameSettings initialized successfully");
        }

        public static GameSettings Instance { get; private set; }

        private void InitializeInstance()
        {
            // setting collection of all game settings
            _settings = new ISetting[]
            {
                new InputBindingsSetting(inputActionAsset),
                new MusicVolumeSetting(audioMixer),
                new SfxVolumeSetting(audioMixer),
                new GraphicSetting(),
                new LanguageSetting(),
                new ResolutionSetting(),
                new FullScreenModeSetting(),
            };

            _storage = new PlayerPrefsSettingsStorage();
        }

        #endregion Temporary solution

        private ISetting[] _settings;
        private ISettingsStorage _storage;

        public void Load()
        {
            foreach (var setting in _settings)
            {
                if (!_storage.HasKey(setting.Id))
                {
                    setting.SetDefault();
                    continue;
                }

                string saveData = _storage.Get(setting.Id);
                setting.Load(saveData);
            }
        }

        public void Save()
        {
            foreach (var setting in _settings)
            {
                string json = JsonUtility.ToJson(setting.Save());
                _storage.Set(setting.Id, json);
            }

            _storage.Save();
        }

        public static T Get<T>() where T : ISetting
        {
            return (T) Instance._settings.First(s => s is T);
        }

        public static void LoadStatic() => Instance.Load();

        public static void SaveStatic() => Instance.Save();
    }
}