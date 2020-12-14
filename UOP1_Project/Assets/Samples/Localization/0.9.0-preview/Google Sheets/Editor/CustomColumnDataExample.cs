using System;
using UnityEditor.Localization.Plugins.Google.Columns;
using UnityEngine.Localization;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Tables;

namespace UnityEditor.Localization.Samples.Google
{
    [Serializable]
    [DisplayName("Custom Data")]
    [Metadata(AllowedTypes = MetadataType.StringTableEntry)]
    public class MyCustomDataMetadata : IMetadata
    {
        public string someValue;
        public string someNoteValue;
    }

    /// <summary>
    /// LocaleMetadataColumn is a version of SheetColumn just for handling Metadata.
    /// This can now be added to the Column Mappings for any Push or Pull request.
    /// </summary>
    public class MyCustomColumn : LocaleMetadataColumn<MyCustomDataMetadata>
    {
        public override PushFields PushFields => PushFields.ValueAndNote; // For our example we use both value and note.

        public override void PullMetadata(StringTableEntry entry, MyCustomDataMetadata metadata, string cellValue, string cellNote)
        {
            // Metadata will be null if the entry does not already contain any.
            if (metadata == null)
            {
                metadata = new MyCustomDataMetadata();
                entry.AddMetadata(metadata);
            }

            metadata.someValue = cellValue;
            metadata.someNoteValue = cellNote;
        }

        public override void PushHeader(StringTableCollection collection, out string header, out string headerNote)
        {
            // The title of the Google Sheet column
            header = "My Custom Data";
            headerNote = null;
        }

        public override void PushMetadata(MyCustomDataMetadata metadata, out string value, out string note)
        {
            // Metadata will never be null as this is only called if the entry contains a metadata entry.
            value = metadata.someValue;
            note = metadata.someNoteValue;
        }
    }
}
