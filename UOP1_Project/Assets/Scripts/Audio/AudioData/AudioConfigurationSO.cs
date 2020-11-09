using UnityEngine;
using UnityEngine.Audio;

//TODO: Check which settings we really need at this level
[CreateAssetMenu(menuName = "Audio/Audio Configuration")]
public class AudioConfigurationSO : ScriptableObject
{
	public AudioMixerGroup OutputAudioMixerGroup = null;
	public bool Mute = false;
	public bool BypassEffects = false;
	public bool BypassListenerEffects = false;
	public bool BypassReverbZones = false;
	public int Priority = 128;
	public float Volume = 1f;
	public float Pitch = 1f;
	public float PanStereo = 0f;
	public float SpatialBlend = 0f;
	public float ReverbZoneMix = 1f;
	public float DopplerLevel = 1f;
	public float Spread = 0f;
	public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
	public float MinDistance = 1f;
	public float MaxDistance = 500f;
	public bool IgnoreListenerVolume = false;
	public bool IgnoreListenerPause = false;
}
