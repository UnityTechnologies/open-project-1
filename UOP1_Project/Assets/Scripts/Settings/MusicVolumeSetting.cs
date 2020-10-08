using System.Globalization;
using Settings.Core;
using UnityEngine.Audio;

namespace Settings
{
    public sealed class MusicVolumeSetting : SettingBase<float>
    {
        private const string MUSIC_VOLUME = "MusicVolume";
        private const float DEFAULT_VALUE = 1;
        private readonly AudioMixer _audioMixer;

        public MusicVolumeSetting(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
        }

        public override float Value
        {
            get
            {
                if (_audioMixer.GetFloat(MUSIC_VOLUME, out float value))
                {
                    return value;
                }

                //TODO: throw exception or log error
                return -1;
            }
            set
            {
                if (_audioMixer.SetFloat(MUSIC_VOLUME, value))
                {
                    OnChanged();
                }
                else
                {
                    //TODO: throw exception or log error
                }
            }
        }

        public override void SetDefault()
        {
            base.SetDefault();
            Value = DEFAULT_VALUE;
        }

        public override void Load(string saveData)
        {
            if (!float.TryParse(saveData, out float value))
            {
                SetDefault();
                return;
            }

            Value = value;
        }

        public override string Save()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}