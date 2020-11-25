using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[Header("SoundEmitters pool")]
	[SerializeField] private SoundEmitterFactorySO _factory;
	[SerializeField] private SoundEmitterPoolSO _pool;
	[SerializeField] private int _initialSize = 10;

	[Header("Listening on channels")]
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
	[SerializeField] private AudioCueEventChannelSO _SFXEventChannel = default;
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
	[SerializeField] private AudioCueEventChannelSO _musicEventChannel = default;


	[Header("Audio control")]
	[SerializeField] private AudioMixer audioMixer;
	[Range(0f, 1f)]
	[SerializeField] private float _masterVolume = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float _musicVolume = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float _sfxVolume = 1f;

	private void Awake()
	{
		//TODO: Get the initial volume levels from the settings

		_SFXEventChannel.OnAudioCueRequested += PlayAudioCue;
		_musicEventChannel.OnAudioCueRequested += PlayAudioCue; //TODO: Treat music requests differently?

		_pool.Prewarm(_initialSize);
	}

	/// <summary>
	/// This is only used in the Editor, to debug volumes.
	/// It is called when any of the variables is changed, and will directly change the value of the volumes on the AudioMixer.
	/// </summary>
	void OnValidate()
	{
		if (Application.isPlaying)
		{
			SetGroupVolume("MasterVolume", _masterVolume);
			SetGroupVolume("MusicVolume", _musicVolume);
			SetGroupVolume("SFXVolume", _sfxVolume);
		}
	}

	public void SetGroupVolume(string parameterName, float normalizedVolume)
	{
		bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
		if (!volumeSet)
			Debug.LogError("The AudioMixer parameter was not found");
	}

	public float GetGroupVolume(string parameterName)
	{
		if (audioMixer.GetFloat(parameterName, out float rawVolume))
		{
			return MixerValueToNormalized(rawVolume);
		}
		else
		{
			Debug.LogError("The AudioMixer parameter was not found");
			return 0f;
		}
	}

	// Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
	/// when using UI sliders normalized format
	private float MixerValueToNormalized(float mixerValue)
	{
		// We're assuming the range [-80dB to 0dB] becomes [0 to 1]
		return 1f + (mixerValue / 80f);
	}
	private float NormalizedToMixerValue(float normalizedValue)
	{
		// We're assuming the range [0 to 1] becomes [-80dB to 0dB]
		// This doesn't allow values over 0dB
		return (normalizedValue - 1f) * 80f;
	}

	/// <summary>
	/// Plays an AudioCue by requesting the appropriate number of SoundEmitters from the pool.
	/// </summary>
	public void PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
	{
		AudioClip[] clipsToPlay = audioCue.GetClips();
		int nOfClips = clipsToPlay.Length;

		for (int i = 0; i < nOfClips; i++)
		{
			SoundEmitter soundEmitter = _pool.Request();
			if (soundEmitter != null)
			{
				soundEmitter.PlayAudioClip(clipsToPlay[i], settings, audioCue.looping, position);
				if (!audioCue.looping)
					soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
			}
		}

		//TODO: Save the SoundEmitters that were activated, to be able to stop them if needed
	}

	private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
	{
		soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
		soundEmitter.Stop();
		_pool.Return(soundEmitter);
	}

	//TODO: Add methods to play and cross-fade music, or to play individual sounds?
}
