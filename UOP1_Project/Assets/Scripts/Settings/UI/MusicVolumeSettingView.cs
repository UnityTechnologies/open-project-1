using Settings.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Settings.UI
{
    public sealed class MusicVolumeSettingView : SettingViewBase<MusicVolumeSetting>
    {
        [SerializeField] private Slider slider = default;

        private void Awake()
        {
            slider.onValueChanged.AddListener((value) => { Setting.Value = value; });
        }

        public override void ResetView()
        {
            slider.value = Setting.Value;
        }
    }
}
