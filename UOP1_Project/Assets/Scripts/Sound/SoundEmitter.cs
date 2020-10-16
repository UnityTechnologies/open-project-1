using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
	private AudioSource thisAudioSource;

	private void Awake()
	{
		DontDestroyOnLoad(this);
		thisAudioSource = this.GetComponent<AudioSource>();
	}

	// Start is called before the first frame update
	void Start()
    {
		thisAudioSource.playOnAwake = false;
    }

	public void PlaySound(AudioClip clip, AudioMixerGroup mixer, Vector3 position, bool loop, float volume, float pitch = 1)
	{
		if (position == Vector3.zero)
		{
			thisAudioSource.spatialBlend = 0;
		}
		else
		{
			thisAudioSource.spatialBlend = 1;
			this.transform.position = position;
		}

		thisAudioSource.clip = clip;
		thisAudioSource.outputAudioMixerGroup = mixer;
		thisAudioSource.loop = loop;
		thisAudioSource.volume = volume;
		thisAudioSource.pitch = pitch;

		thisAudioSource.Play();
	}

	public void StopSound()
	{
		thisAudioSource.Stop();
	}

	public bool IsInUse()
	{
		return thisAudioSource.isPlaying;
	}

	public bool IsLooping()
	{
		return thisAudioSource.loop;
	}
}
