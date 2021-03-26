using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
	[Header("SoundEmitters pool")]
	[SerializeField] private SoundEmitterFactorySO _factory = default;
	[SerializeField] private SoundEmitterPoolSO _pool = default;
	[SerializeField] private int _initialSize = 10;

	[Header("Listening on channels")]
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
	[SerializeField] private AudioCueEventChannelSO _SFXEventChannel = default;
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
	[SerializeField] private AudioCueEventChannelSO _musicEventChannel = default;


	[Header("Audio control")]
	[SerializeField] private AudioMixer audioMixer = default;
	[Range(0f, 1f)]
	[SerializeField] private float _masterVolume = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float _musicVolume = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float _sfxVolume = 1f;

	private SoundEmitterVault _soundEmitterVault;
	private SoundEmitter _musicSoundEmitter;

	private void Awake()
	{
		//TODO: Get the initial volume levels from the settings
		_soundEmitterVault = new SoundEmitterVault();

		_pool.Prewarm(_initialSize);
		_pool.SetParent(this.transform);
	}

	private void OnEnable()
	{
		_SFXEventChannel.OnAudioCuePlayRequested += PlayAudioCue;
		_SFXEventChannel.OnAudioCueStopRequested += StopAudioCue;
		_SFXEventChannel.OnAudioCueFinishRequested += FinishAudioCue;

		_musicEventChannel.OnAudioCuePlayRequested += PlayMusicTrack;
		_musicEventChannel.OnAudioCueStopRequested += StopMusic;
	}

	private void OnDestroy()
	{
		_SFXEventChannel.OnAudioCuePlayRequested -= PlayAudioCue;
		_SFXEventChannel.OnAudioCueStopRequested -= StopAudioCue;
		_SFXEventChannel.OnAudioCueFinishRequested -= FinishAudioCue;

		_musicEventChannel.OnAudioCuePlayRequested -= PlayMusicTrack;
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

	private AudioCueKey PlayMusicTrack(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
	{
		float fadeDuration = 2f;
		float startTime = 0f;

		if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
		{
			AudioClip songToPlay = audioCue.GetClips()[0];
			if (_musicSoundEmitter.GetClip() == songToPlay)
				return AudioCueKey.Invalid;

			//Music is already playing, need to fade it out
			startTime = _musicSoundEmitter.FadeMusicOut(fadeDuration);
		}

		_musicSoundEmitter = _pool.Request();
		_musicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], audioConfiguration, 1f, startTime);
		_musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;

		return AudioCueKey.Invalid; //No need to return a valid key for music
	}

	private bool StopMusic(AudioCueKey key)
	{
		if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
		{
			_musicSoundEmitter.Stop();
			return true;
		}
		else
			return false;
	}

	/// <summary>
	/// Plays an AudioCue by requesting the appropriate number of SoundEmitters from the pool.
	/// </summary>
	public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
	{
		AudioClip[] clipsToPlay = audioCue.GetClips();
		SoundEmitter[] soundEmitterArray = new SoundEmitter[clipsToPlay.Length];

		int nOfClips = clipsToPlay.Length;
		for (int i = 0; i < nOfClips; i++)
		{
			soundEmitterArray[i] = _pool.Request();
			if (soundEmitterArray[i] != null)
			{
				soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings, audioCue.looping, position);
				if (!audioCue.looping)
					soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
			}
		}

		return _soundEmitterVault.Add(audioCue, soundEmitterArray);
	}

	public bool FinishAudioCue(AudioCueKey audioCueKey)
	{
		bool isFound = _soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

		if (isFound)
		{
			for (int i = 0; i < soundEmitters.Length; i++)
			{
				soundEmitters[i].Finish();
				soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
			}
		}
		else
		{
			Debug.LogWarning("Finishing an AudioCue was requested, but the AudioCue was not found.");
		}

		return isFound;
	}

	public bool StopAudioCue(AudioCueKey audioCueKey)
	{
		bool isFound = _soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

		if (isFound)
		{
			for (int i = 0; i < soundEmitters.Length; i++)
			{
				StopAndCleanEmitter(soundEmitters[i]);
			}

			_soundEmitterVault.Remove(audioCueKey);
		}

		return isFound;
	}

	private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
	{
		StopAndCleanEmitter(soundEmitter);
	}

	private void StopAndCleanEmitter(SoundEmitter soundEmitter)
	{
		if (!soundEmitter.IsLooping())
			soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;

		soundEmitter.Stop();
		_pool.Return(soundEmitter);

		//TODO: is the above enough?
		//_soundEmitterVault.Remove(audioCueKey); is never called if StopAndClean is called after a Finish event
		//How is the key removed from the vault?
	}

	private void StopMusicEmitter(SoundEmitter soundEmitter)
	{
		soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
		_pool.Return(soundEmitter);
	}
}
