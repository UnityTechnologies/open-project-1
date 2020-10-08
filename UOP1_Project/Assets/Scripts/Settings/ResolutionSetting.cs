using System;
using System.Collections.Generic;
using System.Linq;
using Settings.Core;
using UnityEngine;

namespace Settings
{
    public sealed class ResolutionSetting : SettingBase<ResolutionSetting.ResolutionData>
    {
        public override ResolutionData Value
        {
            get => new ResolutionData()
            {
                width = Screen.width,
                height = Screen.height
            };

            set
            {
                Screen.SetResolution(value.width, value.height, Screen.fullScreenMode);
                base.Value = value;
            }
        }

        public override void Load(string saveData)
        {
            if (!ResolutionData.TryParse(saveData, out ResolutionData value))
            {
                SetDefault();
                return;
            }

            Value = value;
        }

        public override string Save()
        {
            return Value.ToString();
        }

        public int GetCurrentResolutionIndex()
        {
            ResolutionData[] resolutions = GetAvailableResolutions();
            for (int i = 0; i < resolutions.Length; i++)
            {
                ResolutionData res = resolutions[i];
                if (res.width == Screen.width && res.height == Screen.height)
                    return i;
            }

            Debug.LogError($"Can't find current resolution ({Screen.width}, {Screen.height}) in settings.");
            return 0;
        }

        public void SetResolutionIndex(int index)
        {
            ResolutionData[] resolutions = GetAvailableResolutions();
            if (index < 0 || index >= resolutions.Length)
            {
                Debug.LogError($"Resolution index {index} is out of range ({resolutions.Length})");
                return;
            }

            ResolutionData res = resolutions[index];
            Value = new ResolutionData() {width = res.width, height = res.height};
        }

        // This method could return cached value but I'm not sure if Screen.resolutions always returns the same value.
        public ResolutionData[] GetAvailableResolutions()
        {
            List<ResolutionData> resolutions = new List<ResolutionData>();
            
            // Screen.resolutions returns duplicated resolutions so we need to filter them out
            foreach (Resolution resolution in Screen.resolutions)
            {
                if (resolutions.All(r => r.width != resolution.width && r.height != resolution.height))
                    resolutions.Add(new ResolutionData(){width = resolution.width, height = resolution.height});
            }

            return resolutions.ToArray();
        }

        [Serializable]
        public class ResolutionData : IComparable<ResolutionData>
        {
            public int width;
            public int height;

            public int CompareTo(ResolutionData other)
            {
                if (width != other.width) return -1;
                if (height != other.height) return -1;
                return 0;
            }

            public override string ToString()
            {
                return $"{width} X {height}";
            }

            public static bool TryParse(string s, out ResolutionData value)
            {
                try
                {
                    string[] values = s.Split(new[] {" X "}, StringSplitOptions.RemoveEmptyEntries);
                    int width = int.Parse(values[0]);
                    int height = int.Parse(values[1]);
                    value = new ResolutionData(){width = width, height = height};
                }
                catch
                {
                    value = default;
                    return false;
                }
                return true;
            }
        }
    }
}