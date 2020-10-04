using UnityEngine;

public class SettingsSystem : MonoBehaviour
{
    [SerializeField] SettingsSystemGeneralComponent generalComponent;
    [SerializeField] SettingsSystemGraphicsComponent graphicsComponent;
    [SerializeField] SettingsSystemAudioComponent audioComponent;

    public SettingsSystemGeneralComponent.LanguageSetting Language => generalComponent.Language;
    public bool FullScreen => graphicsComponent.FullScreen;
    public float MusicVolume => audioComponent.MusicVolume;
    public float SfxVolume => audioComponent.SfxVolume;

}