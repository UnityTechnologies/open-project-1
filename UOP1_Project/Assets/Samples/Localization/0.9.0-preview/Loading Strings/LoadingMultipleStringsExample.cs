using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEditor.Localization.Samples
{
    /// <summary>
    /// This example shows how a <see cref="StringTable"/> can be used directly in order to get translated strings for multiple entries.
    /// <seealso cref="LocalizedStringTableExample"/>
    /// </summary>
    public class LoadingMultipleStringsExample : MonoBehaviour
    {
        // A LocalizedStringReference provides a simple interface to retrieving translated strings and their tables.
        public string stringTableCollectionName = "My Strings";

        // We will cache our translated strings
        string m_TranslatedStringHello;
        string m_TranslatedStringGoodbye;
        string m_TranslatedStringThisIsATest;

        void OnEnable()
        {
            // During initialization we start a request for the string and subscribe to any locale change events so that we can update the strings in the future.
            StartCoroutine(LoadStrings());
            LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
        }

        void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
        }

        void OnSelectedLocaleChanged(Locale obj)
        {
            StartCoroutine(LoadStrings());
        }

        IEnumerator LoadStrings()
        {
            // A string table may not be immediately available such as during initialization of the localization system or when a table has not been loaded yet.
            var loadingOperation = LocalizationSettings.StringDatabase.GetTableAsync(stringTableCollectionName);
            yield return loadingOperation;

            if (loadingOperation.Status == AsyncOperationStatus.Succeeded)
            {
                var stringTable = loadingOperation.Result;
                m_TranslatedStringThisIsATest = GetLocalizedString(stringTable, "This is a test");
                m_TranslatedStringHello = GetLocalizedString(stringTable, "Hello");
                m_TranslatedStringGoodbye = GetLocalizedString(stringTable, "Goodbye");
            }
            else
            {
                Debug.LogError("Could not load String Table\n" + loadingOperation.OperationException.ToString());
            }
        }

        string GetLocalizedString(StringTable table, string entryName)
        {
            // Get the table entry. The entry contains the localized string and Metadata
            var entry = table.GetEntry(entryName);
            return entry.GetLocalizedString(); // We can pass in optional arguments for Smart Format or String.Format here
        }

        void OnGUI()
        {
            // We can check if the localization system is ready using the InitializationOperation.
            // Initialization involves loading locales and optionally preloading localized data for the current locale.
            if (!LocalizationSettings.InitializationOperation.IsDone)
            {
                GUILayout.Label("Initializing Localization");
                return;
            }

            GUILayout.Label(m_TranslatedStringThisIsATest);
            GUILayout.Label(m_TranslatedStringHello);
            GUILayout.Label(m_TranslatedStringGoodbye);
        }
    }
}
