using UnityEngine;
using UnityEngine.Localization;
[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Create new settings SO")]

public class SettingsSO : ScriptableObject
{
	[SerializeField] float _masterVolume = default;
	[SerializeField] float _musicVolume = default;
	[SerializeField] float _sfxVolume = default;
	[SerializeField] int _resolutionsIndex = default;
	[SerializeField] int _antiAliasingIndex = default;
	[SerializeField] float _shadowDistance = default;
	[SerializeField] bool _isFullscreen = default;
	[SerializeField] Locale _currentLocale = default;
	public float MasterVolume => _masterVolume;
	public float MusicVolume => _musicVolume;
	public float SfxVolume => _sfxVolume;
	public int ResolutionsIndex => _resolutionsIndex;
	public int AntiAliasingIndex => _antiAliasingIndex;
	public float ShadowDistance => _shadowDistance;
	public bool IsFullscreen => _isFullscreen;
	public Locale CurrentLocale => _currentLocale;
	public void SaveAudioSettings(float newMusicVolume, float newSfxVolume, float newMasterVolume)
	{
		_masterVolume = newMasterVolume;
		_musicVolume = newMusicVolume;
		_sfxVolume = newSfxVolume;
	}
	public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiAliasingIndex, float newShadowDistance, bool fullscreenState)
	{
		_resolutionsIndex = newResolutionsIndex;
		_antiAliasingIndex = newAntiAliasingIndex;
		_shadowDistance = newShadowDistance;
		_isFullscreen = fullscreenState;
	}
	public void SaveLanguageSettings(Locale local)
	{
		_currentLocale = local;
	}
	public SettingsSO() { }
	public void LoadSavedSettings(Save savedFile)
	{
		_masterVolume = savedFile._masterVolume;
		_musicVolume = savedFile._musicVolume;
		_sfxVolume = savedFile._sfxVolume;
		_resolutionsIndex = savedFile._resolutionsIndex;
		_antiAliasingIndex = savedFile._antiAliasingIndex;
		_shadowDistance = savedFile._shadowDistance;
		_isFullscreen = savedFile._isFullscreen;
		_currentLocale = savedFile._currentLocale;
	}
}
