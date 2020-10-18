using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	public static SoundManager singleton;

	[Header ("Init Variables")]
	[Tooltip ("Amount of sound emitters created on Start")]
	public int initialPoolSize = 1;

	[Header ("Mixer Groups")]
	public AudioMixerGroup masterMixer;
	public AudioMixerGroup sfxMixer;
	public AudioMixerGroup musicMixer;

	private List<SoundEmitter> soundEmitterPool = new List<SoundEmitter>();
	private GameObject soundEmittersContainer;

	private void Awake()
	{
		DontDestroyOnLoad(this);

		if (!SoundManager.singleton)
			singleton = this;
		else
		{
			Debug.LogWarning("One sound manager already present in the game, destroying " + this.name + ".");
			Destroy(this);
		}
	}

	void Start()
    {
		InitEmitterPool();
    }

	#region mixer groups functions
	public static bool SetGroupVolume(AudioMixerGroup group, float volume)
	{
		return group.audioMixer.SetFloat("Volume", NormalizedToMixerValue(volume));
	}
	
	public static bool GetGroupVolume(AudioMixerGroup group, out float volume)
	{
	        if(group.audioMixer.GetFloat("Volume", out float rawVolume)){
	                volume = MixerValueNormalized(rawVolume);
	                return true;
	        }
	        volume = default;
	        return false;
	}

	#region mixerHelpers
	// Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations when using UI sliders normalized format
	private static float MixerValueNormalized(float value)
	{
		return  (-(value - 80) / 80) - 1;
	}
	private static float NormalizedToMixerValue(float normalizedValue)
	{
		return -80 + (normalizedValue * 80);
	}
	#endregion
	#endregion

	#region play sounds functions
	public SoundEmitter Play2DSound(AudioClip clip, AudioMixerGroup mixer, bool loop = false, float volume = 1, float pitch = 1)
	{
		if (!mixer)
			mixer = masterMixer;

		SoundEmitter foundSoundEmit = GetSoundEmitter();

		foundSoundEmit.PlaySound(clip, mixer, Vector3.zero, loop, volume, pitch, 0);

		return foundSoundEmit;
	}

	public SoundEmitter PlaySpatialSound(AudioClip clip, AudioMixerGroup mixer, Vector3 position, bool loop = false, float volume = 1, float pitch = 1, float spartialBlend = 1)
	{
		if (!mixer)
			mixer = masterMixer;

		SoundEmitter foundSoundEmit = GetSoundEmitter();

		foundSoundEmit.PlaySound(clip, mixer, position, loop, volume, pitch, spartialBlend);

		return foundSoundEmit;
	}

	public void StopAllSounds()
	{
		foreach (SoundEmitter emit in soundEmitterPool)
		{
			if (emit.IsInUse())
   				emit.StopSound();
		}
	}
	public void StopAllNonLoopSounds()
	{
		foreach (SoundEmitter emit in soundEmitterPool)
		{
			if (emit.IsInUse() && !emit.IsLooping())
				emit.StopSound();
		}
	}
	public void StopAllLoopSounds()
	{
		foreach (SoundEmitter emit in soundEmitterPool)
		{
			if (emit.IsInUse() && emit.IsLooping())
				emit.StopSound();
		}
	}
	#endregion

	#region SoundEmitters Pool Functions
	private void InitEmitterPool()
	{
		soundEmittersContainer = new GameObject();
		soundEmittersContainer.name = "Sound Emitters Pool";
		DontDestroyOnLoad(soundEmittersContainer);

		for (int i = 0; i < initialPoolSize; i++)
		{
			CreateNewSoundEmitter();
		}
	}

	private SoundEmitter CreateNewSoundEmitter()
	{
		if (soundEmitterPool.Count >= AudioSettings.GetConfiguration().numVirtualVoices)
			throw new UnityException("Error creating a new SoundEmitter instance, this sound will not be played. Sound emitter pool already has enough instances to cover the number of virtual voices.");

		GameObject temp = new GameObject();
		var emitter = temp.AddComponent<SoundEmitter>();
		temp.name = "SoundEmitter #" + soundEmitterPool.Count;

		temp.transform.SetParent(soundEmittersContainer.transform);

		soundEmitterPool.Add(emitter);
		return emitter;
	}

	// GetSoundEmitter with return the next SoundEmitter that is not playing any sounds, if all the sound emitters are busy it will extend the pool
	private SoundEmitter GetSoundEmitter()
	{
		return soundEmitterPool.FirstOrDefault(emitter => !emitter.IsInUse()) ?? CreateNewSoundEmitter();
	}
	#endregion
}
