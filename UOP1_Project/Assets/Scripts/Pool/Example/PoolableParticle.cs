using UOP1.Pool;
using System;
using System.Collections;
using UnityEngine;

public class PoolableParticle : MonoBehaviour, IPoolable
{
	[SerializeField]
	private ParticleSystem _particleSystem = default;

	public void OnRequest()
	{
		gameObject.SetActive(true);
	}

	public void Play()
	{
		_particleSystem.Play();
	}

	public void OnReturn(Action onReturned)
	{
		StartCoroutine(DoReturn(onReturned));
	}

	IEnumerator DoReturn(Action onReturned)
	{
		if (_particleSystem.isPlaying)
		{
			yield return new WaitForSeconds(_particleSystem.main.duration - (_particleSystem.time % _particleSystem.main.duration));
			_particleSystem.Stop();
		}
		yield return new WaitUntil(() => _particleSystem.particleCount == 0);
		onReturned.Invoke();
		gameObject.SetActive(false);
	}
}
