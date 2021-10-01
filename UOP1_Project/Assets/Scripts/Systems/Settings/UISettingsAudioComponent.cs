using UnityEngine;
using UnityEngine.Events;

public class UISettingsAudioComponent : MonoBehaviour
{
	[SerializeField] UISettingItemFiller _masterVolumeField;
	[SerializeField] UISettingItemFiller _musicVolumeField;
	[SerializeField] UISettingItemFiller _sfxVolumeField;

	[SerializeField] UIGenericButton _saveButton;
	[SerializeField] UIGenericButton _resetButton;

	[Header("Broadcasting")]
	[SerializeField] private FloatEventChannelSO _masterVolumeEventChannel = default;
	[SerializeField] private FloatEventChannelSO _sFXVolumeEventChannel = default;
	[SerializeField] private FloatEventChannelSO _musicVolumeEventChannel = default;
	private float _musicVolume { get; set; }
	private float _sfxVolume { get; set; }
	private float _masterVolume { get; set; }
	private float _savedMusicVolume { get; set; }
	private float _savedSfxVolume { get; set; }
	private float _savedMasterVolume { get; set; }

	int _maxVolume = 10;

	public event UnityAction<float, float, float> _save = delegate { };
	private void OnEnable()
	{
		_musicVolumeField.OnNextOption += IncreaseMusicVolume;
		_musicVolumeField.OnPreviousOption += DecreaseMusicVolume;
		_saveButton.Clicked += SaveVolumes;
		_resetButton.Clicked += ResetVolumes;
		_sfxVolumeField.OnNextOption += IncreaseSFXVolume;
		_sfxVolumeField.OnPreviousOption += DecreaseSFXVolume;
		_masterVolumeField.OnNextOption += IncreaseMasterVolume;
		_masterVolumeField.OnPreviousOption += DecreaseMasterVolume;

	}
	private void OnDisable()
	{
		ResetVolumes(); // reset volumes on disable. If not saved, it will reset to initial volumes. 
		_musicVolumeField.OnNextOption -= IncreaseMusicVolume;
		_musicVolumeField.OnPreviousOption -= DecreaseMusicVolume;
		_saveButton.Clicked -= SaveVolumes;
		_resetButton.Clicked -= ResetVolumes;
		_sfxVolumeField.OnNextOption -= IncreaseSFXVolume;
		_sfxVolumeField.OnPreviousOption -= DecreaseSFXVolume;
		_masterVolumeField.OnNextOption -= IncreaseMasterVolume;
		_masterVolumeField.OnPreviousOption -= DecreaseMasterVolume;

	}
	public void Setup(float musicVolume, float sfxVolume, float masterVolume)
	{
		_masterVolume = masterVolume;
		_musicVolume = sfxVolume;
		_sfxVolume = musicVolume;

		_savedMasterVolume = _masterVolume;
		_savedMusicVolume = _musicVolume;
		_savedSfxVolume = _sfxVolume;

		SetMusicVolumeField();
		SetSfxVolumeField();
		SetMasterVolumeField();
	}
	private void SetMusicVolumeField()
	{
		int paginationCount = _maxVolume + 1; // adding a page in the pagination since the count starts from 0
		int selectedPaginationIndex = Mathf.RoundToInt(_maxVolume * _musicVolume);
		string selectedOption = Mathf.RoundToInt(_maxVolume * _musicVolume).ToString();

		SetMusicVolume();

		_musicVolumeField.FillSettingField(paginationCount, selectedPaginationIndex, selectedOption);


	}
	private void SetSfxVolumeField()
	{
		int paginationCount = _maxVolume + 1;// adding a page in the pagination since the count starts from 0
		int selectedPaginationIndex = Mathf.RoundToInt(_maxVolume * _sfxVolume);
		string selectedOption = Mathf.RoundToInt(_maxVolume * _sfxVolume).ToString();

		SetSfxVolume();

		_sfxVolumeField.FillSettingField(paginationCount, selectedPaginationIndex, selectedOption);

	}
	private void SetMasterVolumeField()
	{
		int paginationCount = _maxVolume + 1;// adding a page in the pagination since the count starts from 0
		int selectedPaginationIndex = Mathf.RoundToInt(_maxVolume * _masterVolume);
		string selectedOption = Mathf.RoundToInt(_maxVolume * _masterVolume).ToString();

		SetMasterVolume();

		_masterVolumeField.FillSettingField(paginationCount, selectedPaginationIndex, selectedOption);

	}
	private void SetMusicVolume()
	{
		_musicVolumeEventChannel.RaiseEvent(_musicVolume);//raise event for volume change
	}
	private void SetSfxVolume()
	{
		_sFXVolumeEventChannel.RaiseEvent(_sfxVolume); //raise event for volume change

	}
	private void SetMasterVolume()
	{
		_masterVolumeEventChannel.RaiseEvent(_masterVolume); //raise event for volume change

	}

	private void IncreaseMasterVolume()
	{
		_masterVolume += 1 / (float)_maxVolume;
		_masterVolume = Mathf.Clamp(_masterVolume, 0, 1);
		SetMasterVolumeField();
	}
	private void DecreaseMasterVolume()
	{
		_masterVolume -= 1 / (float)_maxVolume;
		_masterVolume = Mathf.Clamp(_masterVolume, 0, 1);
		SetMasterVolumeField();
	}
	private void IncreaseMusicVolume()
	{
		_musicVolume += 1 / (float)_maxVolume;
		_musicVolume = Mathf.Clamp(_musicVolume, 0, 1);
		SetMusicVolumeField();
	}
	private void DecreaseMusicVolume()
	{

		_musicVolume -= 1 / (float)_maxVolume;
		_musicVolume = Mathf.Clamp(_musicVolume, 0, 1);
		SetMusicVolumeField();
	}
	private void IncreaseSFXVolume()
	{
		_sfxVolume += 1 / (float)_maxVolume;
		_sfxVolume = Mathf.Clamp(_sfxVolume, 0, 1);

		SetSfxVolumeField();
	}
	private void DecreaseSFXVolume()
	{

		_sfxVolume -= 1 / (float)_maxVolume;
		_sfxVolume = Mathf.Clamp(_sfxVolume, 0, 1);
		SetSfxVolumeField();
	}

	private void ResetVolumes()
	{
		Setup(_savedMusicVolume, _savedSfxVolume, _savedMasterVolume);
	}
	private void SaveVolumes()
	{
		_savedMasterVolume = _masterVolume;
		_savedMusicVolume = _musicVolume;
		_savedSfxVolume = _sfxVolume;
		//save Audio
		_save.Invoke(_musicVolume, _sfxVolume, _masterVolume);
	}


}
