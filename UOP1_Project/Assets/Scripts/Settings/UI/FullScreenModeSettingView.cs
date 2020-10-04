using Settings.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Settings.UI
{
    public class FullScreenModeSettingView : SettingViewBase<FullScreenModeSetting>
    {
        [SerializeField] private Toggle toggle = default;

        private void Awake()
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool value)
        {
            Setting.Value = value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }

        public override void ResetView()
        {
            toggle.SetIsOnWithoutNotify(Setting.Value == FullScreenMode.FullScreenWindow);
        }
    }
}