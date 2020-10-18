using System.Collections;
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
	private int poolIndex = 0;

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
		SoundEmitter toReturn = null;
		int startIndex = poolIndex;
		poolIndex++;

		// Cycles through the list of emitters until it finds one in use or has completed a full cycle
		while (toReturn == null)
		{
			if (poolIndex >= soundEmitterPool.Count)
				poolIndex = 0;

			if (poolIndex == startIndex) // If none of the sound emitters are available, create a new sound emitter and assign the index to the newly created one
			{
				toReturn = CreateNewSoundEmitter();
				poolIndex = soundEmitterPool.Count - 1;
			}

			if (!soundEmitterPool[poolIndex].IsInUse())
			{
				toReturn = soundEmitterPool[poolIndex];
			}
			else
				poolIndex++;
		}

		return toReturn;
	}
	#endregion
}
