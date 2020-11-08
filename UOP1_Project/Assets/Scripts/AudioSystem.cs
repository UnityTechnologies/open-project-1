using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AudioSystem : MonoBehaviour
{

	private bool _sfxMute;
	private bool _musicMute;
	private AudioSource _theme;
	private AudioSource _sfx;
	private static readonly List<string> mixBuffer = new List<string>();
	private const float mixBufferClearDelay = 0.05f;

	internal string currentTrack;

	public float delayInCrossfading = 0.3f;
	public List<Theme> tracks = new List<Theme>();
	public List<SFX> sounds = new List<SFX>();

	private static int BoolToBinary(bool b) => b ? 1 : 0;
	private SFX GetSoundByName(string sName) => sounds.Find(x => x.name == sName);

	public float MusicVolume => !PlayerPrefs.HasKey("MusicVolume") ? 1f : PlayerPrefs.GetFloat("MusicVolume");
	public float SfxVolume => !PlayerPrefs.HasKey("SFXVolume") ? 1f : PlayerPrefs.GetFloat("SFXVolume");

	private void Awake()
	{
		// Configuring Audio Source For Playing Music And SFX
		_theme = gameObject.AddComponent<AudioSource>();
		_theme.loop = true;
		_sfx = gameObject.AddComponent<AudioSource>();
		_sfxMute = false;
		_musicMute = false;
		// Check If Sfx Volume Is Not 0
		if (Math.Abs(SfxVolume) > 0.05f)
			_sfx.volume = SfxVolume;
		// Set The Values To 0
		else
			_sfx.volume = 0;
		// Check If Music Volume Is Not 0
		if (Math.Abs(MusicVolume) > 0.05f)
			_theme.volume = MusicVolume;
		// Set The Values To 0
		else
			_theme.volume = 0;
		// Checks If The sfxMute Is True Or Not
		if (PlayerPrefs.GetInt("sfxMute") == 1)
		{
			SfxToggle();
		}
		// Checks If The musicMute Is True Or Not
		if (PlayerPrefs.GetInt("musicMute") == 1)
		{
			MusicToggle();
		}

		StartCoroutine(MixBufferRoutine());
	}

	// Responsible for limiting the frequency of playing sounds
	private IEnumerator MixBufferRoutine()
	{
		float time = 0;

		while (true)
		{
			time += Time.unscaledDeltaTime;
			yield return 0;
			if (time >= mixBufferClearDelay)
			{
				mixBuffer.Clear();
				time = 0;
			}
		}
	}

	// Play a music track with Cross fading
	public void PlayMusic(string trackName)
	{
		if (trackName != "")
			currentTrack = trackName;
		AudioClip to = null;
		foreach (Theme track in tracks)
			if (track.name == trackName)
				to = track.track;

		StartCoroutine(CrossFade(to));
	}

	public void StopSound()
	{
		_sfx.Stop();
	}

	// Cross fading - Smooth Transition When Track Is Switched
	private IEnumerator CrossFade(AudioClip to)
	{
		if (_theme.clip != null)
		{
			while (delayInCrossfading > 0)
			{
				_theme.volume = delayInCrossfading * MusicVolume;
				delayInCrossfading -= Time.unscaledDeltaTime;
				yield return 0;
			}
		}
		_theme.clip = to;
		if (to == null)
		{
			_theme.Stop();
			yield break;
		}
		delayInCrossfading = 0;

		if (!_theme.isPlaying)
			_theme.Play();

		while (delayInCrossfading < 1f)
		{
			_theme.volume = delayInCrossfading * MusicVolume;
			delayInCrossfading += Time.unscaledDeltaTime;
			yield return 0;
		}
		_theme.volume = MusicVolume;
	}

	// Sfx Button On/Off
	public void SfxToggle()
	{
		_sfxMute = !_sfxMute;
		_sfx.mute = _sfxMute;
		PlayerPrefs.SetInt("sfxMute", BoolToBinary(_sfxMute));
		PlayerPrefs.Save();
	}

	// Music Button On/Off
	public void MusicToggle()
	{
		_musicMute = !_musicMute;
		_theme.mute = _musicMute;
		PlayerPrefs.SetInt("musicMute", BoolToBinary(_musicMute));
		PlayerPrefs.Save();
	}

	// A single sound effect
	public void PlaySound(string clip)
	{
		SFX sound = GetSoundByName(clip);

		if (sound != null && !mixBuffer.Contains(clip))
		{
			if (sound.clips.Count == 0)
				return;
			mixBuffer.Add(clip);
			_sfx.PlayOneShot(sound.clips[Random.Range(0, sound.clips.Count - 1)]);
		}
	}
}

[Serializable]
public class Theme
{
	public string name;
	public AudioClip track;
}

[Serializable]
public class SFX
{
	public string name;
	public List<AudioClip> clips = new List<AudioClip>();
}
