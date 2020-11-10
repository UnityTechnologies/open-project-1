using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[Tooltip("Amount of sound emitters created on Start")]
	[SerializeField] private int _initialPoolSize = 1;
	[SerializeField] private SoundEmitter _soundEmitterPrefab = default;
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
	[SerializeField] private AudioCueEventSO _SFXEvent = default;
	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
	[SerializeField] private AudioCueEventSO _musicEvent = default;

	private SoundEmitterFactorySO _factory;
	private SoundEmitterPoolSO _pool;
	public SoundEmitterPoolSO Pool { get { return _pool; } }

	private void Awake()
	{
		InitPool();

		_SFXEvent.eventRaised += PlayAudioCue;
	}

	private void InitPool()
	{
		_factory = ScriptableObject.CreateInstance<SoundEmitterFactorySO>();
		_factory.Prefab = _soundEmitterPrefab;
		_factory.Prefab.name = "SoundEmitter Factory";
		_pool = ScriptableObject.CreateInstance<SoundEmitterPoolSO>();
		_pool.name = "SoundEmitter Pool";
		_pool.Factory = _factory;
		_pool.InitialPoolSize = _initialPoolSize;
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
				soundEmitter.PlaySound(clipsToPlay[i], settings, audioCue.looping, position);
				if (audioCue.looping)
					soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
			}
		}

		//TODO: Save the SoundEmitters that were activated, to be able to stop them if needed
	}

	private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
	{
		soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
		Pool.Return(soundEmitter);
	}

	//TODO: Add methods to play and cross-fade music, or to play individual sounds?
}
