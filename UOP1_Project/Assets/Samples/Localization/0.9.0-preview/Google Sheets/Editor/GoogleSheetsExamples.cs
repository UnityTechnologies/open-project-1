using System.Linq;
using UnityEditor.Localization.Plugins.Google;
using UnityEditor.Localization.Plugins.Google.Columns;
using UnityEditor.Localization.Reporting;
using UnityEngine;

namespace UnityEditor.Localization.Samples.Google
{
    /// <summary>
    /// These examples show various ways to sync a String Table Collection with Google Sheets.
    /// These examples are illustrative and will not work as they are without correct Google Sheets credentials and String Table data.
    /// </summary>
    public class GoogleSheetsExamples
    {
        static SheetsServiceProvider GetServiceProvider()
        {
            // The Sheets service provider performs the authentication and keeps track of the
            // authentication tokens so that we do not need to authenticate each time.
            // It is recommended to have a SheetsServiceProvider asset pre-configured for
            // use however in this example we will create a new one.
            var sheetServiceProvider = ScriptableObject.CreateInstance<SheetsServiceProvider>();

            // OAuth is required when making changes. See the docs for info setting up OAuth credentials.
            sheetServiceProvider.SetOAuthCrendtials("some-client-id", "some-client-secret");
            return sheetServiceProvider;
        }

        /// <summary>
        /// It is possible for a String Table Collection to exist over several Google Sheets, for example one per Locale.
        /// This example shows how you could push 1 of those Locales
        /// </summary>
        [MenuItem("Localization Samples/Google Sheets/Push English")]
        public static void PushEnglish()
        {
            // Setup the connection to Google
            var sheetServiceProvider = GetServiceProvider();
            var googleSheets = new GoogleSheets(sheetServiceProvider);
            googleSheets.SpreadSheetId = "My spread sheet id"; // We need to provide the Spreadsheet id. This can be found in the url. See docs for further info.

            // Prepare the data we want to push.
            // You should provide your String Table Collection name here
            var tableCollection = LocalizationEditorSettings.GetStringTableCollection("My Strings");

            // We need configure what each column will contain in the sheet
            var columnMappings = new SheetColumn[]
            {
                // Column A will contain the Key
                new KeyColumn { Column = "A" },

                // Column B will contain any shared comments. These are Comment Metadata in the Shared category.
                new KeyCommentColumn { Column = "B" },

                // Column C will contain the English Locale and any comments that are just for this Locale.
                new LocaleColumn { Column = "C", LocaleIdentifier = "en", IncludeComments = true },
            };

            int mySheetId = 123456; // This it the id of the sheet in the Google Spreadsheet. it will be in the url after `gid=`.

            // Now send the update.
            googleSheets.PushStringTableCollection(mySheetId, tableCollection, columnMappings);
        }

        /// <summary>
        /// It is possible for a String Table Collection to exist over several Google Sheets, for example one per Locale.
        /// This example shows how you could pull 1 of those Locales into a String Table Collection.
        /// </summary>
        [MenuItem("Localization Samples/Google Sheets/Pull English")]
        public static void PullEnglish()
        {
            // Setup the connection to Google
            var sheetServiceProvider = GetServiceProvider();
            var googleSheets = new GoogleSheets(sheetServiceProvider);
            googleSheets.SpreadSheetId = "My spread sheet id"; // We need to provide the Spreadsheet id. This can be found in the url. See docs for further info.

            // You should provide your String Table Collection name here
            var tableCollection = LocalizationEditorSettings.GetStringTableCollection("My Strings");

            // We need configure what each column contains in the sheet
            var columnMappings = new SheetColumn[]
            {
                // Column A contains the Key
                new KeyColumn { Column = "A" },

                // Column B contains any shared comments. These are Comment Metadata in the Shared category.
                new KeyCommentColumn { Column = "B" },

                // Column C contains the English Locale and any comments that are just for this Locale.
                new LocaleColumn { Column = "C", LocaleIdentifier = "en", IncludeComments = true },
            };

            int mySheetId = 123456; // This it the id of the sheet in the Google Spreadsheet. it will be in the url after `gid=`.
            googleSheets.PullIntoStringTableCollection(mySheetId, tableCollection, columnMappings);
        }

        /// <summary>
        /// This example shows how you can Push all the Locales in your project by using the ColumnMapper to generate the ColumnMapping data for you.
        /// </summary>
        [MenuItem("Localization Samples/Google Sheets/Push Project Locales")]
        public static void PushProjectLocales()
        {
            // Setup the connection to Google
            var sheetServiceProvider = GetServiceProvider();
            var googleSheets = new GoogleSheets(sheetServiceProvider);
            googleSheets.SpreadSheetId = "My spread sheet id"; // We need to provide the Spreadsheet id. This can be found in the url. See docs for further info.

            // Prepare the data we want to push.
            // You should provide your String Table Collection name here
            var tableCollection = LocalizationEditorSettings.GetStringTableCollection("My Strings");

            // CreateDefaultMapping will create a KeyColumn and a LocaleColumn for each Locale in the project.
            var columnMappings = ColumnMapping.CreateDefaultMapping();
            int mySheetId = 123456; // This it the id of the sheet in the Google Spreadsheet. it will be in the url after `gid=`.

            // Now send the update. We can pass in an optional ProgressBarReporter so that we can see updates in the Editor.
            googleSheets.PushStringTableCollection(mySheetId, tableCollection, columnMappings, new ProgressBarReporter());
        }

        /// <summary>
        /// This example shows how you can Pull all the Locales in your project by using the ColumnMapper to generate the ColumnMapping data for you.
        /// </summary>
        [MenuItem("Localization Samples/Google Sheets/Pull Project Locales")]
        public static void PullProjectLocales()
        {
            // Setup the connection to Google
            var sheetServiceProvider = GetServiceProvider();
            var googleSheets = new GoogleSheets(sheetServiceProvider);
            googleSheets.SpreadSheetId = "My spread sheet id"; // We need to provide the Spreadsheet id. This can be found in the url. See docs for further info.

            // You should provide your String Table Collection name here
            var tableCollection = LocalizationEditorSettings.GetStringTableCollection("My Strings");

            // CreateDefaultMapping will create a KeyColumn and a LocaleColumn for each Locale in the project.
            // This assumes that the table was created and pushed to using the same column mappings.
            var columnMappings = ColumnMapping.CreateDefaultMapping();
            int mySheetId = 123456; // This it the id of the sheet in the Google Spreadsheet. it will be in the url after `gid=`.

            // Now pull.
            // removeMissingEntries will remove any Keys that we have in the String Table Collection that do not exist in the Pull update.
            // reporter is an optional reporter that can be used to povide feedback in the editor during the Pull.
            googleSheets.PullIntoStringTableCollection(mySheetId, tableCollection, columnMappings, removeMissingEntries: true, reporter: new ProgressBarReporter());
        }

        static void PushExtension(GoogleSheetsExtension googleExtension)
        {
            // Setup the connection to Google
            var googleSheets = new GoogleSheets(googleExtension.SheetsServiceProvider);
            googleSheets.SpreadSheetId = googleExtension.SpreadsheetId;

            // Now send the update. We can pass in an optional ProgressBarReporter so that we can updates in the Editor.
            googleSheets.PushStringTableCollection(googleExtension.SheetId, googleExtension.TargetCollection as StringTableCollection, googleExtension.Columns, new ProgressBarReporter());
        }

        static void PullExtension(GoogleSheetsExtension googleExtension)
        {
            // Setup the connection to Google
            var googleSheets = new GoogleSheets(googleExtension.SheetsServiceProvider);
            googleSheets.SpreadSheetId = googleExtension.SpreadsheetId;

            // Now update the collection. We can pass in an optional ProgressBarReporter so that we can updates in the Editor.
            googleSheets.PullIntoStringTableCollection(googleExtension.SheetId, googleExtension.TargetCollection as StringTableCollection, googleExtension.Columns, reporter: new ProgressBarReporter());
        }

        /// <summary>
        /// In this example we use the data that was configured in the Google Sheets extension to perform a Push.
        /// </summary>
        [MenuItem("Localization Samples/Google Sheets/Push With Google Extension")]
        public static void PushWithExtension()
        {
            // You should provide your String Table Collection name here
            var tableCollection = LocalizationEditorSettings.GetStringTableCollection("My Strings");
            var googleExtension = tableCollection.Extensions.FirstOrDefault(e => e is GoogleSheetsExtension) as GoogleSheetsExtension;
            if (googleExtension == null)
            {
                Debug.LogError($"String Table Collection {tableCollection.TableCollectionName} Does not contain a Google Sheets Extension.");
                return;
            }

            PushExtension(googleExtension);
        }

        /// <summary>
        /// In this example we use the data that was configured in the Google Sheets extension to perform a Pull.
        /// </summary>
        [MenuItem("Localization Samples/Google Sheets/Pull With Google Extension")]
        public static void PullWithExtension()
        {
            // You should provide your String Table Collection name here
            var tableCollection = LocalizationEditorSettings.GetStringTableCollection("My Strings");
            var googleExtension = tableCollection.Extensions.FirstOrDefault(e => e is GoogleSheetsExtension) as GoogleSheetsExtension;
            if (googleExtension == null)
            {
                Debug.LogError($"String Table Collection {tableCollection.TableCollectionName} Does not contain a Google Sheets Extension.");
                return;
            }

            PullExtension(googleExtension);
        }

        /// <summary>
        /// This example shows how we can push every String Table Collection that contains a Google Sheets extension.
        /// </summary>
        [MenuItem("Localization Samples/Google Sheets/Push All Google Sheets Extensions")]
        public static void PushAllExtensions()
        {
            // Get every String Table Collection
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (var collection in stringTableCollections)
            {
                // Its possible a String Table Collection may have more than one GoogleSheetsExtension.
                // For example if each Locale we pushed/pulled from a different sheet.
                foreach (var extension in collection.Extensions)
                {
                    if (extension is GoogleSheetsExtension googleExtension)
                    {
                        PushExtension(googleExtension);
                    }
                }
            }
        }

        /// <summary>
        /// This example shows how we can push every String Table Collection that contains a Google Sheets extension.
        /// </summary>
        [MenuItem("Localization Samples/Google Sheets/Pull All Google Sheets Extensions")]
        public static void PullAllExtensions()
        {
            // Get every String Table Collection
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (var collection in stringTableCollections)
            {
                // Its possible a String Table Collection may have more than one GoogleSheetsExtension.
                // For example if each Locale we pushed/pulled from a different sheet.
                foreach (var extension in collection.Extensions)
                {
                    if (extension is GoogleSheetsExtension googleExtension)
                    {
                        PullExtension(googleExtension);
                    }
                }
            }
        }
    }
}
