using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEditor.Localization.Samples
{
    /// <summary>
    /// This example shows how to get a localized string using the StringDatabase.
    /// This example also shows some of the ways to wait for an AsyncOperationHandle to complete.
    /// </summary>
    public class StringDatabaseGetLocalizedStringExample : MonoBehaviour
    {
        public bool useCoroutine;

        void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
            UpdateString();
        }

        void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
        }

        void SelectedLocaleChanged(Locale locale)
        {
            UpdateString();
        }

        void UpdateString()
        {
            // Sometimes the localized value may not be immediately available.
            // The Localization system may not have been initialized yet or the String Table may need loading.
            // The AsyncOperation wraps this loading operation. We can yield on it in a coroutine,
            // use its various Completed Events or await its Task if using async and await.
            var stringOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Start Game");
            if (stringOperation.IsDone)
                SetString(stringOperation);

            if (useCoroutine)
                StartCoroutine(LoadStringWithCoroutine(stringOperation));
            else
                stringOperation.Completed += SetString;
        }

        IEnumerator LoadStringWithCoroutine(AsyncOperationHandle<string> stringOperation)
        {
            yield return stringOperation;
            SetString(stringOperation);
        }

        void SetString(AsyncOperationHandle<string> stringOperation)
        {
            // Its possible that something may have gone wrong during loading. We can handle this locally
            // or ignore all errors as they will still be captured and reported by the Localization system.
            if (stringOperation.Status == AsyncOperationStatus.Failed)
                Debug.LogError("Failed to load string");
            else
                Debug.Log("Loaded String: " + stringOperation.Result);
        }
    }
}
