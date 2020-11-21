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
	public SoundEmitterPoolSO Pool { get { return _pool; } }

	private void Awake()
	{
		_SFXEventChannel.OnAudioCueRequested += PlayAudioCue;
		Pool.Prewarm(_initialSize);
	}

	public static bool SetGroupVolume(AudioMixerGroup group, float volume)
	{
		return group.audioMixer.SetFloat("Volume", NormalizedToMixerValue(volume));
	}

	public static bool GetGroupVolume(AudioMixerGroup group, out float volume)
	{
		if (group.audioMixer.GetFloat("Volume", out float rawVolume))
		{
			volume = MixerValueNormalized(rawVolume);
			return true;
		}
		volume = default;
		return false;
	}

	// Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations when using UI sliders normalized format
	private static float MixerValueNormalized(float value)
	{
		return (-(value - 80) / 80) - 1;
	}
	private static float NormalizedToMixerValue(float normalizedValue)
	{
		return -80 + (normalizedValue * 80);
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
		Pool.Return(soundEmitter);
	}

	//TODO: Add methods to play and cross-fade music, or to play individual sounds?
}
