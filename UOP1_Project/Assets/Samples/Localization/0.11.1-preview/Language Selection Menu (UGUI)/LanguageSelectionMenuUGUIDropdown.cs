using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UnityEngine.Localization.Samples
{
    /// <summary>
    /// This example shows how a language selection menu can be implemented using UGUI.
    /// </summary>
    [RequireComponent(typeof(Dropdown))]
    public class LanguageSelectionMenuUGUIDropdown : MonoBehaviour
    {
        Dropdown m_Dropdown;
        AsyncOperationHandle m_InitializeOperation;

        void Start()
        {
            // First we setup the dropdown component.
            m_Dropdown = GetComponent<Dropdown>();
            m_Dropdown.onValueChanged.AddListener(OnSelectionChanged);

            // Clear the options an add a loading message while we wait for the localization system to initialize.
            m_Dropdown.ClearOptions();
            m_Dropdown.options.Add(new Dropdown.OptionData("Loading..."));
            m_Dropdown.interactable = false;

            // SelectedLocaleAsync will ensure that the locales have been initialized and a locale has been selected.
            m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;
            if (m_InitializeOperation.IsDone)
            {
                InitializeCompleted(m_InitializeOperation);
            }
            else
            {
                m_InitializeOperation.Completed += InitializeCompleted;
            }
        }

        void InitializeCompleted(AsyncOperationHandle obj)
        {
            // Create an option in the dropdown for each Locale
            var options = new List<string>();
            int selectedOption = 0;
            var locales = LocalizationSettings.AvailableLocales.Locales;
            for (int i = 0; i < locales.Count; ++i)
            {
                var locale = locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selectedOption = i;

                var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
                options.Add(displayName);
            }

            // If we have no Locales then something may have gone wrong.
            if (options.Count == 0)
            {
                options.Add("No Locales Available");
                m_Dropdown.interactable = false;
            }
            else
            {
                m_Dropdown.interactable = true;
            }

            m_Dropdown.ClearOptions();
            m_Dropdown.AddOptions(options);
            m_Dropdown.SetValueWithoutNotify(selectedOption);

            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        }

        void OnSelectionChanged(int index)
        {
            // Unsubscribe from SelectedLocaleChanged so we don't get an unnecessary callback from the change we are about to make.
            LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

            var locale = LocalizationSettings.AvailableLocales.Locales[index];
            LocalizationSettings.SelectedLocale = locale;

            // Resubscribe to SelectedLocaleChanged so that we can stay in sync with changes that may be made by other scripts.
            LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        }

        void LocalizationSettings_SelectedLocaleChanged(Locale locale)
        {
            // We need to update the dropdown selection to match.
            var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            m_Dropdown.SetValueWithoutNotify(selectedIndex);
        }
    }
}
