using System.Collections.Generic;
using Settings.Core;
using Settings.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Settings.UI
{
    /// <summary>
    /// Base class for any setting view that uses Dropdown.
    /// </summary>
    /// <typeparam name="TSetting">Setting.</typeparam>
    public abstract class DropdownSettingViewBase<TSetting> : SettingViewBase<TSetting> where TSetting : ISetting
    {
        [SerializeField] private Dropdown dropdown = default;

        private void Awake()
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        public override void ResetView()
        {
            dropdown.options = GetOptions();
            dropdown.SetValueWithoutNotify(GetCurrentValue());
        }

        protected abstract void OnDropdownValueChanged(int index);

        protected abstract List<Dropdown.OptionData> GetOptions();

        protected abstract int GetCurrentValue();
    }
}