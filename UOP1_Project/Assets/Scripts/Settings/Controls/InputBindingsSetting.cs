using System;
using System.Collections.Generic;
using Settings.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Settings.Controls
{
    public class InputBindingsSetting : SettingBase<InputBindingsSetting.BindingsOverridesList>
    {
        private readonly InputActionAsset _inputActionAsset;

        public InputBindingsSetting(InputActionAsset inputActionAsset)
        {
            _inputActionAsset = inputActionAsset;
        }

        public override BindingsOverridesList Value
        {
            get => GetControlOverrides();
            set => SetControlOverrides(value);
        }

        public override void SetDefault()
        {
            base.SetDefault();
            Value = new BindingsOverridesList();
        }

        /// <summary>
        /// Private wrapper class for json serialization of the overrides
        /// </summary>
        [System.Serializable]
        public class BindingsOverridesList
        {
            public List<BindingSerializable> bindingList = new List<BindingSerializable>();
        }

        /// <summary>
        /// Internal struct to store an {id, overridePath} pair for a list
        /// </summary>
        [System.Serializable]
        public struct BindingSerializable
        {
            public string id;
            public string path;

            public BindingSerializable(string bindingId, string bindingPath)
            {
                id = bindingId;
                path = bindingPath;
            }
        }

        public override bool IsValueEqual(BindingsOverridesList a, BindingsOverridesList b)
        {
            return false;
        }

        public override string Save()
        {
            return JsonUtility.ToJson(Value);
        }

        public override void Load(string saveData)
        {
            try
            {
                Value = JsonUtility.FromJson<BindingsOverridesList>(saveData);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError(
                    "Exception thrown when tried to load input bindings. Setting default binding settings...");
                SetDefault();
            }
        }

        /// <summary>
        /// Gets bindings' overrides
        /// </summary>
        private BindingsOverridesList GetControlOverrides()
        {
            BindingsOverridesList bindingsList = new BindingsOverridesList();
            foreach (var map in _inputActionAsset.actionMaps)
            {
                foreach (var binding in map.bindings)
                {
                    if (!string.IsNullOrEmpty(binding.overridePath))
                    {
                        bindingsList.bindingList.Add(
                            new BindingSerializable(binding.id.ToString(), binding.overridePath));
                    }
                }
            }

            return bindingsList;
        }

        /// <summary>
        /// Loads control overrides
        /// </summary>
        private void SetControlOverrides(BindingsOverridesList bindingsList)
        {
            //create a dictionary to easier check for existing overrides
            Dictionary<System.Guid, string> overrides = new Dictionary<System.Guid, string>();
            foreach (var item in bindingsList.bindingList)
            {
                overrides.Add(new System.Guid(item.id), item.path);
            }

            //walk through action maps check dictionary for overrides
            foreach (var map in _inputActionAsset.actionMaps)
            {
                // we want to load binding overrides into map, that is clean from previous overrides
                map.RemoveAllBindingOverrides();

                var bindings = map.bindings;
                for (var i = 0; i < bindings.Count; ++i)
                {
                    if (overrides.TryGetValue(bindings[i].id, out string overridePath))
                    {
                        Debug.Log($"Override input: {overridePath}");
                        //if there is an override apply it
                        map.ApplyBindingOverride(i, new InputBinding {overridePath = overridePath});
                    }
                }
            }
        }
    }
}