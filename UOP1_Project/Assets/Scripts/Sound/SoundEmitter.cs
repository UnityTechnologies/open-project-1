using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
	private AudioSource _audioSource;

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

	public void PlaySound(AudioClip clip, AudioMixerGroup mixer, Vector3 position, bool loop, float volume, float pitch, float spartialBlend)
	{
		this.transform.position = position;

		_audioSource.clip = clip;
		_audioSource.outputAudioMixerGroup = mixer;
		_audioSource.loop = loop;
		_audioSource.volume = volume;
		_audioSource.pitch = pitch;
		_audioSource.spatialBlend = spartialBlend;	

		_audioSource.Play();
	}

	public void StopSound()
	{
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
}
