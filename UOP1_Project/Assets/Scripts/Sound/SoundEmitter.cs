using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UOP1.Pool;
using System;

[RequireComponent (typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour, IPoolable
{
	private AudioSource _audioSource;

	private float _lastUseTimestamp = 0;

	private void Awake()
	{
		DontDestroyOnLoad(this);
		_audioSource = this.GetComponent<AudioSource>();
	}

	// Start is called before the first frame update
	void Start()
    {
		_audioSource.playOnAwake = false;
    }

	public void OnRequest()
	{
		gameObject.SetActive(true);
	}

	public void OnReturn(Action onReturned)
	{
		StopSound();
		gameObject.SetActive(false);
	}

	public void PlaySound(AudioClip clip, SoundEmitterSettings settings, Vector3 position = default)
	{
		_audioSource.clip = clip;
		ApplySettings(_audioSource, settings);
		_audioSource.transform.position = position;
		_lastUseTimestamp = settings.Loop ? Mathf.Infinity : Time.realtimeSinceStartup + clip.length;
		_audioSource.Play();

		if (!settings.Loop)
		{
			StartCoroutine(FinishedPlaying(clip.length));
		}
	}

	private void ApplySettings(AudioSource source, SoundEmitterSettings settings)
	{
		source.outputAudioMixerGroup = settings.OutputAudioMixerGroup;
		source.mute = settings.Mute;
		source.bypassEffects = settings.BypassEffects;
		source.bypassListenerEffects = settings.BypassListenerEffects;
		source.bypassReverbZones = settings.BypassReverbZones;
		source.loop = settings.Loop;
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
		SoundManager.Instance.Pool.Return(this);
	}
}
