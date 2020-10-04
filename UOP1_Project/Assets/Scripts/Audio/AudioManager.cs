using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	private AudioSource _audioSource;

	public static AudioManager Instance { get; private set; }

	private void Awake()
	{
		PreventMultipleAudioManagers();
		_audioSource = GetComponent<AudioSource>();
	}

	public void Play(AudioClip audioClip, AudioMixerGroup audioMixerGroup = null)
	{
		_audioSource.outputAudioMixerGroup = audioMixerGroup;
		_audioSource.clip = audioClip;
		_audioSource.Play();
	}

	public void PlayOneShot(AudioClip audioClip, float volume = 1f)
	{
		_audioSource.PlayOneShot(audioClip, volume);
	}

	public void Stop()
	{
		_audioSource.Stop();
	}

	public void SetAudioMixerGroup(AudioMixerGroup audioMixerGroup)
	{
		_audioSource.outputAudioMixerGroup = audioMixerGroup;
	}

	private void PreventMultipleAudioManagers()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
}
