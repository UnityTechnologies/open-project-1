using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class UISettingsLanguageComponent : MonoBehaviour
{
	[SerializeField] private UISettingItemFiller _languageField = default;
	[SerializeField] private UIGenericButton _saveButton;
	[SerializeField] private UIGenericButton _resetButton;

	public event UnityAction<Locale> _save = delegate { };
	
	private int _currentSelectedOption = 0;
	private int _savedSelectedOption = default;
	private AsyncOperationHandle _initializeOperation;
	private List<string> _languagesList = new List<string>();

	void OnEnable()
	{
		_initializeOperation = LocalizationSettings.SelectedLocaleAsync;
		if (_initializeOperation.IsDone)
		{
			InitializeCompleted(_initializeOperation);
		}
		else
		{
			_initializeOperation.Completed += InitializeCompleted;
		}
		_saveButton.Clicked += SaveSettings;
		_resetButton.Clicked += ResetSettings;
		_languageField.OnNextOption += NextOption;
		_languageField.OnPreviousOption += PreviousOption;
	}

	private void OnDisable()
	{
		ResetSettings();

		_saveButton.Clicked -= SaveSettings;
		_resetButton.Clicked -= ResetSettings;
		_languageField.OnNextOption -= NextOption;
		_languageField.OnPreviousOption -= PreviousOption;
		LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
	}

	void InitializeCompleted(AsyncOperationHandle obj)
	{
		_initializeOperation.Completed -= InitializeCompleted;
		// Create an option in the dropdown for each Locale
		_languagesList = new List<string>();

		List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;

		for (int i = 0; i < locales.Count; ++i)
		{
			var locale = locales[i];
			if (LocalizationSettings.SelectedLocale == locale)
				_currentSelectedOption = i;

			var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
			_languagesList.Add(displayName);
		}
		_languageField.FillSettingField(_languagesList.Count, _currentSelectedOption, _languagesList[_currentSelectedOption]);
		_savedSelectedOption = _currentSelectedOption;
		LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
	}

	void NextOption()
	{
		_currentSelectedOption++;
		_currentSelectedOption = Mathf.Clamp(_currentSelectedOption, 0, _languagesList.Count - 1);
		OnSelectionChanged();
	}

	void PreviousOption()
	{
		_currentSelectedOption--;
		_currentSelectedOption = Mathf.Clamp(_currentSelectedOption, 0, _languagesList.Count - 1);
		OnSelectionChanged();
	}

	void OnSelectionChanged()
	{
		// Unsubscribe from SelectedLocaleChanged so we don't get an unnecessary callback from the change we are about to make.
		LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

		var locale = LocalizationSettings.AvailableLocales.Locales[_currentSelectedOption];
		LocalizationSettings.SelectedLocale = locale;

		// Resubscribe to SelectedLocaleChanged so that we can stay in sync with changes that may be made by other scripts.
		LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
	}

	void LocalizationSettings_SelectedLocaleChanged(Locale locale)
	{
		// We need to update the dropdown selection to match.
		var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
		_languageField.FillSettingField(_languagesList.Count, selectedIndex, _languagesList[selectedIndex]);
	}

	public void SaveSettings()
	{
		Locale _currentLocale = LocalizationSettings.AvailableLocales.Locales[_currentSelectedOption];
		_savedSelectedOption = _currentSelectedOption;
		_save.Invoke(_currentLocale);
	}

	public void ResetSettings()
	{
		_currentSelectedOption = _savedSelectedOption;
		OnSelectionChanged();
	}
}
