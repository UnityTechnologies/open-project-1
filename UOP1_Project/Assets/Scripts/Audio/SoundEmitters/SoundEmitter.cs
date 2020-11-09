using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UOP1.Pool;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour, IPoolable
{
	private AudioSource _audioSource;
	private float _lastUseTimestamp = 0;

	public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

	private void Awake()
	{
		_audioSource = this.GetComponent<AudioSource>();
		_audioSource.playOnAwake = false;
	}

	public void OnRequest() { }

	public void OnReturn(Action onReturned)
	{
		StopSound();
	}

	/// <summary>
	/// Instructs the AudioSource to play a single clip, with optional looping, in a position in 3D space.
	/// </summary>
	/// <param name="clip"></param>
	/// <param name="settings"></param>
	/// <param name="hasToLoop"></param>
	/// <param name="position"></param>
	public void PlaySound(AudioClip clip, AudioConfigurationSO settings, bool hasToLoop, Vector3 position = default)
	{
		_audioSource.clip = clip;
		ApplySettings(_audioSource, settings);
		_audioSource.transform.position = position;
		_audioSource.loop = hasToLoop;
		_audioSource.Play();

		if (!hasToLoop)
		{
			StartCoroutine(FinishedPlaying(clip.length));
		}
	}

	private void ApplySettings(AudioSource source, AudioConfigurationSO settings)
	{
		source.outputAudioMixerGroup = settings.OutputAudioMixerGroup;
		source.mute = settings.Mute;
		source.bypassEffects = settings.BypassEffects;
		source.bypassListenerEffects = settings.BypassListenerEffects;
		source.bypassReverbZones = settings.BypassReverbZones;
		source.priority = settings.Priority;
		source.volume = settings.Volume;
		source.pitch = settings.Pitch;
		source.panStereo = settings.PanStereo;
		source.spatialBlend = settings.SpatialBlend;
		source.reverbZoneMix = settings.ReverbZoneMix;
		source.dopplerLevel = settings.DopplerLevel;
		source.spread = settings.Spread;
		source.rolloffMode = settings.RolloffMode;
		source.minDistance = settings.MinDistance;
		source.maxDistance = settings.MaxDistance;
		source.ignoreListenerVolume = settings.IgnoreListenerVolume;
		source.ignoreListenerPause = settings.IgnoreListenerPause;
	}

	public void StopSound()
	{
		_lastUseTimestamp = Time.realtimeSinceStartup;
		_audioSource.Stop();
	}

	public bool IsInUse()
	{
		return _audioSource.isPlaying;
	}

	public bool IsLooping()
	{
		return _audioSource.loop;
	}

	public float LastUseTimestamp()
	{
		return _lastUseTimestamp;
	}

	IEnumerator FinishedPlaying(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);

		OnSoundFinishedPlaying.Invoke(this); // The AudioManager will pick this up
	}
}
