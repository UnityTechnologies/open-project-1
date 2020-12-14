using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ParticleEffectConfig
{
	public ParticleEffectSO particleEffect;
	public Transform effectAnchor;

	// Keep track of the particle instance of each effect managed by EffectController
	private ParticleSystem _particle;
	public ParticleSystem Particle
	{
		get
		{
			return _particle;
		}
		set
		{
			_particle = value;
		}
	}
}

[ExecuteInEditMode]
public class EffectController : MonoBehaviour
{
	[SerializeField] private List<ParticleEffectConfig> _particleConfigs;
	private List<ParticleSystem> _particles;

	public List<ParticleEffectConfig> ParticleConfigs
	{
		get
		{
			return _particleConfigs;
		}
	}

	public void PlayEffect(ParticleEffectSO effect)
	{
		foreach (ParticleEffectConfig config in _particleConfigs)
		{
			if (config.particleEffect == effect)
			{
				if (config.Particle != null)
				{
					config.particleEffect.Pool.Return(config.Particle);
				}
				config.Particle = config.particleEffect.Pool.Request(config.effectAnchor);
				StartCoroutine(TriggerEffectCoroutine(config));
			}
		}
	}

	public void StopEffect(ParticleEffectSO effect)
	{
		foreach (ParticleEffectConfig config in _particleConfigs)
		{
			if (config.particleEffect == effect)
			{
				if (config.Particle != null)
				{
					config.Particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
			}
		}
	}

	private IEnumerator TriggerEffectCoroutine(ParticleEffectConfig particleConfig)
	{
		particleConfig.Particle.Play();

		while (particleConfig.Particle.IsAlive())
		{
			yield return null;
		}
		particleConfig.particleEffect.Pool.Return(particleConfig.Particle);
	}
}
