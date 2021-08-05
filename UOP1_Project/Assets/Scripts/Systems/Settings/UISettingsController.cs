using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;
public enum SettingsType
{
	Language,
	Graphics,
	Audio,
}
public class UISettingsController : MonoBehaviour
{
	[SerializeField] private SettingsLanguageComponent _languageComponent;
	[SerializeField] private SettingsGraphicsComponent _graphicsComponent;
	[SerializeField] private SettingsAudioComponent _audioComponent;
	[SerializeField] private UISettingTabsFiller _settingTabFiller = default;
	[SerializeField] private SettingsSO _currentSettings;
	[SerializeField] private List<SettingsType> _settingTabsList = new List<SettingsType>();
	private SettingsType _selectedTab = SettingsType.Audio;
	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private VoidEventChannelSO SaveSettingsEvent = default;
	public UnityAction Closed;
	private void OnEnable()
	{
		_languageComponent._save += SaveLaguageSettings;
		_audioComponent._save += SaveAudioSettings;
		_graphicsComponent._save += SaveGraphicsSettings;

		_inputReader.menuCloseEvent += CloseScreen;
		_inputReader.TabSwitched += SwitchTab;

		_settingTabFiller.FillTabs(_settingTabsList);
		_settingTabFiller.ChooseTab += OpenSetting;

		OpenSetting(SettingsType.Audio);

	}
	private void OnDisable()
	{
		_inputReader.menuCloseEvent -= CloseScreen;
		_inputReader.TabSwitched -= SwitchTab;

		_languageComponent._save -= SaveLaguageSettings;
		_audioComponent._save -= SaveAudioSettings;
		_graphicsComponent._save -= SaveGraphicsSettings;
	}
	public void CloseScreen()
	{
		Closed.Invoke();
	}



	void OpenSetting(SettingsType settingType)
	{
		_selectedTab = settingType;
		switch (settingType)
		{
			case SettingsType.Language:
				_currentSettings.SaveLanguageSettings(_currentSettings.CurrentLocale);
				break;
			case SettingsType.Graphics:
				_graphicsComponent.Setup();
				break;
			case SettingsType.Audio:
				_audioComponent.Setup(_currentSettings.MusicVolume, _currentSettings.SfxVolume, _currentSettings.MasterVolume);
				break;
			default:
				break;
		}

		_languageComponent.gameObject.SetActive(settingType == SettingsType.Language);
		_graphicsComponent.gameObject.SetActive((settingType == SettingsType.Graphics));
		_audioComponent.gameObject.SetActive(settingType == SettingsType.Audio);
		_settingTabFiller.SelectTab(settingType);

	}
	void SwitchTab(float orientation)
	{

		if (orientation != 0)
		{
			bool isLeft = orientation < 0;
			int initialIndex = _settingTabsList.FindIndex(o => o == _selectedTab);
			if (initialIndex != -1)
			{
				if (isLeft)
				{
					initialIndex--;
				}
				else
				{
					initialIndex++;
				}

				initialIndex = Mathf.Clamp(initialIndex, 0, _settingTabsList.Count - 1);
			}

			OpenSetting(_settingTabsList[initialIndex]);
		}
	}
	public void SaveLaguageSettings(Locale local)
	{
		_currentSettings.SaveLanguageSettings(local);
		SaveSettingsEvent.RaiseEvent();
	}
	public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiAliasingIndex, float newShadowDistance, bool fullscreenState)
	{
		_currentSettings.SaveGraphicsSettings(newResolutionsIndex, newAntiAliasingIndex, newShadowDistance, fullscreenState);
		SaveSettingsEvent.RaiseEvent();
	}
	void SaveAudioSettings(float musicVolume, float sfxVolume, float masterVolume)
	{
		_currentSettings.SaveAudioSettings(musicVolume, sfxVolume, masterVolume);

		SaveSettingsEvent.RaiseEvent();
	}

}
