using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
	private AudioSource audioSource;

	private void Awake()
	{
		DontDestroyOnLoad(this);
		audioSource = this.GetComponent<AudioSource>();
	}

	// Start is called before the first frame update
	void Start()
    {
		audioSource.playOnAwake = false;
    }

	public void PlaySound(AudioClip clip, AudioMixerGroup mixer, Vector3 position, bool loop, float volume, float pitch, float spartialBlend)
	{
		this.transform.position = position;

		audioSource.clip = clip;
		audioSource.outputAudioMixerGroup = mixer;
		audioSource.loop = loop;
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		audioSource.spatialBlend = spartialBlend;	

		audioSource.Play();
	}

	public void StopSound()
	{
		audioSource.Stop();
	}

	public bool IsInUse()
	{
		return audioSource.isPlaying;
	}

	public bool IsLooping()
	{
		return audioSource.loop;
	}
}
