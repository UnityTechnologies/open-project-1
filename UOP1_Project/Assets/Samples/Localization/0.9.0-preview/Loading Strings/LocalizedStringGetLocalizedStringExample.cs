using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEditor.Localization.Samples
{
    /// <summary>
    /// This example shows how to use GetLocalizedString to retrieve a localized string.
    /// </summary>
    public class LocalizedStringGetLocalizedStringExample : MonoBehaviour
    {
		// A LocalizedString provides an interface to retrieving translated strings.
		// This example assumes a String Table Collection with the name "My String Table" and an entry with the Key "Hello World" exists.
		// You can change the Table Collection and Entry target in the inspector.
		public LocalizedString stringRef; 
        void OnGUI()
        {
		
			// This will make a request to the StringDatabase each time using the LocalizedString properties.
			var stringOperation = stringRef.GetLocalizedString();
            if (stringOperation.IsDone && stringOperation.Status == AsyncOperationStatus.Succeeded)
                GUILayout.Label(stringOperation.Result);
        }
		private void Start()
		{
			stringRef = new LocalizedString() { TableReference = "QuestlineDialogue", TableEntryReference = "SD-L1-S1-Q1-QL1" };

		}
	}
}
