using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSystemAudioComponent : SettingsSystemSubComponents
{
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    
    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }

    void Start()
    {
        Setup();
    }

    protected override void Setup()
    {
        //TODO: load previous music volume setting
        MusicVolume = 50f;
        musicVolumeSlider.SetValueWithoutNotify(MusicVolume);
        //TODO: load previous sfx volume setting
        SfxVolume = 50f;
        sfxVolumeSlider.SetValueWithoutNotify(SfxVolume);
    }
    
    #region UI CALLBACKS
    
    //TODO: clamp volume to [0, 1] or [0, 100]? Change Slider min max value in editor depending on use case
    public void OnChangeMusicVolume(float volume)
    {
        MusicVolume = Mathf.Clamp(volume, 0.0f, 100.0f);
    }

    //TODO: clamp volume to [0, 1] or [0, 100]? Change Slider min max value in editor depending on use case
    public void OnChangeSfxVolume(float volume)
    {
        SfxVolume = Mathf.Clamp(volume, 0.0f, 100.0f);
    }
    
    #endregion
}
