using System.Globalization;
using Settings.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace Settings
{
    public class SfxVolumeSetting : SettingBase<float>
    {
        private readonly AudioMixer _audioMixer;

        public SfxVolumeSetting(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
        }
        public override float Value
        {
            get => AudioListener.volume; // todo: set value to AudioMixer
            set
            {
                AudioListener.volume = value; // todo
                base.Value = value;
            }
        }

        public override void SetDefault()
        {
            base.SetDefault();
            Value = 1;
        }

        public override void Load(string saveData)
        {
            if (!float.TryParse(saveData, out var value))
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