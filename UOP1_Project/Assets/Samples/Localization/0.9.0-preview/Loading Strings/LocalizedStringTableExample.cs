using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace UnityEditor.Localization.Samples
{
    /// <summary>
    /// This example shows how a <see cref="StringTable"> can be used directly in order to get translated
    /// strings for multiple entries using a <see cref="LocalizedStringTable>"/>.
    /// </summary>
    public class LocalizedStringTableExample : MonoBehaviour
    {
        public LocalizedStringTable stringTable = new LocalizedStringTable { TableReference = "My Strings" };

        // We will cache our translated strings
        string m_TranslatedStringHello;
        string m_TranslatedStringGoodbye;
        string m_TranslatedStringThisIsATest;

        void OnEnable()
        {
            stringTable.TableChanged += LoadStrings;
        }

        void OnDisable()
        {
            stringTable.TableChanged -= LoadStrings;
        }

        void LoadStrings(StringTable stringTable)
        {
            m_TranslatedStringHello = GetLocalizedString(stringTable, "Hello");
            m_TranslatedStringGoodbye = GetLocalizedString(stringTable, "Goodbye");
            m_TranslatedStringThisIsATest = GetLocalizedString(stringTable, "This is a test");
        }

        static string GetLocalizedString(StringTable table, string entryName)
        {
            // Get the table entry. The entry contains the localized string and Metadata
            var entry = table.GetEntry(entryName);
            return entry.GetLocalizedString(); // We can pass in optional arguments for Smart Format or String.Format here.
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
