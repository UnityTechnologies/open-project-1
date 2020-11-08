using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager_Elocutura : MonoBehaviour
{
	public static SoundManager_Elocutura Instance;

	[Header ("Init Variables")]
	[Tooltip ("Amount of sound emitters created on Start")]
	[SerializeField]
	private int initialPoolSize = 1;
	[SerializeField]
	private SoundEmitter soundEmitterPrefab = default;

	SoundEmitterFactorySO _factory;
	SoundEmitterPoolSO _pool;
	public SoundEmitterPoolSO Pool { get { return _pool; } }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		Instance = default;
	}

	private void Awake()
	{
		if (SoundManager_Elocutura.Instance)
		{
			Debug.LogWarning($"One sound manager already present in the game, destroying {name}.");
			Destroy(this);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this);

			InitPool();
		}
	}

	private void InitPool()
	{
		_factory = ScriptableObject.CreateInstance<SoundEmitterFactorySO>();
		_factory.Prefab = soundEmitterPrefab;
		_factory.Prefab.name = "SoundEmitter Factory";
		_pool = ScriptableObject.CreateInstance<SoundEmitterPoolSO>();
		_pool.name = "SoundEmitter Pool";
		_pool.Factory = _factory;
		_pool.InitialPoolSize = initialPoolSize;
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
	public SoundEmitter PlaySound(AudioClip clip, SoundEmitterSettings settings, Vector3 position = default)
	{
		SoundEmitter soundEmitter = _pool.Request();
		if (soundEmitter != null)
		{
			soundEmitter.PlaySound(clip, settings, position);
		}
		return soundEmitter;
	}
	#endregion
}
