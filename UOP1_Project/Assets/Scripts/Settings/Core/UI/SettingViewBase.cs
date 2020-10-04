using UnityEngine;

namespace Settings.Core.UI
{
    public abstract class SettingViewBase<TSetting> : MonoBehaviour where TSetting : ISetting
    {
        protected virtual TSetting Setting => GameSettings.Get<TSetting>();

        public abstract void ResetView();
        
        protected virtual void OnEnable()
        {
            ResetView();
            GameSettings.Get<TSetting>().Changed += OnSettingChanged;
        }

        protected virtual void OnDisable()
        {
            GameSettings.Get<TSetting>().Changed -= OnSettingChanged;
        }

        protected virtual void OnSettingChanged()
        {
            ResetView();
        }
    }
}