using OP1.Pool;
using System;
using System.Collections;
using UnityEngine;

public class PoolableParticle : MonoBehaviour, IPoolable
{
	[SerializeField]
	ParticleSystem _particleSystem = default;

	public void Initialize()
	{
		gameObject.SetActive(true);
	}

	public void Play()
	{
		_particleSystem.Play();
	}

	public void Reset(Action onReset)
	{
		StartCoroutine(DoReset(onReset));
	}

	IEnumerator DoReset(Action onReset)
	{
		if (_particleSystem.isPlaying)
		{
			yield return new WaitForSecondsRealtime(_particleSystem.main.duration - (_particleSystem.time % _particleSystem.main.duration));
			_particleSystem.Stop();
		}
		yield return new WaitUntil(() => _particleSystem.particleCount == 0);
		onReset.Invoke();
		gameObject.SetActive(false);
	}


}
