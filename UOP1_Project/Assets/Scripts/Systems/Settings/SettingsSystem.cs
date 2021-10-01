using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.Universal;

public class SettingsSystem : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO SaveSettingsEvent = default;

	[SerializeField] private SettingsSO _currentSettings = default;
	[SerializeField] private UniversalRenderPipelineAsset _urpAsset = default;
	[SerializeField] private SaveSystem _saveSystem = default;

	[SerializeField] private FloatEventChannelSO _changeMasterVolumeEventChannel = default;
	[SerializeField] private FloatEventChannelSO _changeSFXVolumeEventChannel = default;
	[SerializeField] private FloatEventChannelSO _changeMusicVolumeEventChannel = default;

	private void Awake()
	{
		_saveSystem.LoadSaveDataFromDisk();
		_currentSettings.LoadSavedSettings(_saveSystem.saveData);
		SetCurrentSettings();
	}
	private void OnEnable()
	{
		SaveSettingsEvent.OnEventRaised += SaveSettings;
	}
	private void OnDisable()
	{
		SaveSettingsEvent.OnEventRaised -= SaveSettings;
	}
	/// <summary>
	/// Set current settings 
	/// </summary>
	void SetCurrentSettings()
	{
		_changeMusicVolumeEventChannel.RaiseEvent(_currentSettings.MusicVolume);//raise event for volume change
		_changeSFXVolumeEventChannel.RaiseEvent(_currentSettings.SfxVolume); //raise event for volume change
		_changeMasterVolumeEventChannel.RaiseEvent(_currentSettings.MasterVolume); //raise event for volume change
		Resolution currentResolution = Screen.currentResolution; // get a default resolution in case saved resolution doesn't exist in the resolution List
		if (_currentSettings.ResolutionsIndex < Screen.resolutions.Length)
			currentResolution = Screen.resolutions[_currentSettings.ResolutionsIndex];
		Screen.SetResolution(currentResolution.width, currentResolution.height, _currentSettings.IsFullscreen);
		_urpAsset.shadowDistance = _currentSettings.ShadowDistance;
		_urpAsset.msaaSampleCount = _currentSettings.AntiAliasingIndex;

		LocalizationSettings.SelectedLocale = _currentSettings.CurrentLocale;
	}
	void SaveSettings()
	{
		_saveSystem.SaveDataToDisk();
	}





}

