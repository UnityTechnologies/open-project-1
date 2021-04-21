using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Localization;
#endif

public static class LocalizationUtils
{
#if UNITY_EDITOR

	/// <summary>
	/// (Editor only)
	/// Gets a locale to use in edit mode in the editor.
	/// </summary>
	/// <param name="tableCollection">Optional table collection with which to filter the available locales.</param>
	/// <returns>The locale, null if none usable found.</returns>

	static Locale Editor_GetValidLocaleInEditMode(LocalizationTableCollection tableCollection)
	{
		foreach (var locale in LocalizationEditorSettings.GetLocales())
		{
			if (locale != null && (tableCollection == null || tableCollection.GetTable(locale.Identifier) != null))
				return locale;
		}

		return null;
	}

#endif

	/// <summary>
	/// Gets the localized string from an already loaded table, taking into account whether we are in edit mode, play mode, or a build.
	/// </summary>
	/// <param name="localizedStringReference">The <see cref="LocalizedString"/>.</param>
	/// <returns>The localized string.</returns>

	public static string GetLocalizedStringImmediateSafe(this LocalizedString localizedStringReference)
	{
		// If we are in the editor in edit mode, we need to find a valid locale and get the localized string from it:
#if UNITY_EDITOR
		if (EditorApplication.isPlaying)
			return String.Empty;

		string text = null;
		if (!localizedStringReference.IsEmpty)
		{
			var tableCollection = LocalizationEditorSettings.GetStringTableCollection(localizedStringReference.TableReference);
			Locale locale = Editor_GetValidLocaleInEditMode(tableCollection);
			if (locale != null)
			{
				StringTable table = (StringTable)tableCollection.GetTable(locale.Identifier);
				if (table != null)
					text = table.GetEntryFromReference(localizedStringReference.TableEntryReference).LocalizedValue;
			}
		}
		return text;
#endif

		// At runtime (build or editor in play mode), we just get the localized string normally:
		return localizedStringReference.GetLocalizedString().Result;
	}
}
