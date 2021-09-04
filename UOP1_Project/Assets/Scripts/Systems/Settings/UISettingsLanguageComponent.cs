using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class UISettingsLanguageComponent : MonoBehaviour
{
	[FormerlySerializedAs("languageDropdown")]
	[SerializeField] UISettingItemFiller _languageField = default;

	AsyncOperationHandle m_InitializeOperation;
	List<string> languageList = new List<string>();

	public event UnityAction<Locale, bool> _save = delegate { };
	private int _currentSelectedOption = 0;
	private int _savedSelectedOption = default;

	[SerializeField] private UISettingItemFiller _captioningField = default;
	private bool _isCaptioningEnabled = default;
	private bool _savedIsCaptioningEnabled = default;

	[SerializeField] private UIGenericButton _saveButton;
	[SerializeField] private UIGenericButton _resetButton;


	void OnEnable()
	{
		m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;
		if (m_InitializeOperation.IsDone)
		{
			InitializeCompleted(m_InitializeOperation);
		}
		else
		{
			m_InitializeOperation.Completed += InitializeCompleted;
		}
		_saveButton.Clicked += SaveSettings;
		_resetButton.Clicked += ResetSettings;

		_languageField._nextOption += NextOption;
		_languageField._previousOption += PreviousOption;

		_captioningField._nextOption += NextIsCaptioningEnabledState;
		_captioningField._previousOption += PreviousIsCaptioningEnabledState;
	}
	private void OnDisable()
	{
		ResetSettings();

		_saveButton.Clicked -= SaveSettings;
		_resetButton.Clicked -= ResetSettings;
		_languageField._nextOption -= NextOption;
		_languageField._previousOption -= PreviousOption;
		LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
	}

	public void Setup(bool isCaptioningEnabled)
	{
		_isCaptioningEnabled = isCaptioningEnabled;
		_savedIsCaptioningEnabled = _isCaptioningEnabled;

		SetCaptioning();		
	}	

	void InitializeCompleted(AsyncOperationHandle obj)
	{
		m_InitializeOperation.Completed -= InitializeCompleted;
		// Create an option in the dropdown for each Locale
		languageList = new List<string>();

		List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;

		for (int i = 0; i < locales.Count; ++i)
		{
			var locale = locales[i];
			if (LocalizationSettings.SelectedLocale == locale)
				_currentSelectedOption = i;

			var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
			languageList.Add(displayName);
		}
		_languageField.FillSettingField(languageList.Count, _currentSelectedOption, languageList[_currentSelectedOption]);
		_savedSelectedOption = _currentSelectedOption;
		LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
	}

	void NextOption()
	{
		_currentSelectedOption++;
		Debug.Log(_currentSelectedOption);
		_currentSelectedOption = Mathf.Clamp(_currentSelectedOption, 0, languageList.Count - 1);
		OnSelectionChanged();
	}
	void PreviousOption()
	{
		_currentSelectedOption--;
		_currentSelectedOption = Mathf.Clamp(_currentSelectedOption, 0, languageList.Count - 1);
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
		_languageField.FillSettingField(languageList.Count, selectedIndex, languageList[selectedIndex]);

	}
	public void SaveSettings()
	{
		Locale _currentLocale = LocalizationSettings.AvailableLocales.Locales[_currentSelectedOption];
		_savedSelectedOption = _currentSelectedOption;
		_savedIsCaptioningEnabled = _isCaptioningEnabled;
		_save.Invoke(_currentLocale, _isCaptioningEnabled);
	}
	public void ResetSettings()
	{
		_currentSelectedOption = _savedSelectedOption;
		OnSelectionChanged();

		_isCaptioningEnabled = _savedIsCaptioningEnabled;		
		OnCaptioningChange();
	}

	#region IsCaptioningEnabled
	void NextIsCaptioningEnabledState()
	{
		_isCaptioningEnabled = true;
		OnCaptioningChange();
	}
	void PreviousIsCaptioningEnabledState()
	{
		_isCaptioningEnabled = false;
		OnCaptioningChange();
	}
	void OnCaptioningChange()
	{
		SetCaptioning();
	}

	private void SetCaptioning()
	{
		if (_isCaptioningEnabled)
		{
			_captioningField.FillSettingField_Localized(2, 1, "On");
		}
		else
		{
			_captioningField.FillSettingField_Localized(2, 0, "Off");
		}	
	}
	#endregion
}
