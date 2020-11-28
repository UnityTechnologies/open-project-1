using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPoolTester : MonoBehaviour
{
	[SerializeField]
	private int _initialPoolSize = 5;

	private ParticlePoolSO _pool;
	private ParticleFactorySO _factory;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	private IEnumerator Start()
	{
		_factory = ScriptableObject.CreateInstance<ParticleFactorySO>();
		_pool = ScriptableObject.CreateInstance<ParticlePoolSO>();
		_pool.name = gameObject.name;
		_pool.Factory = _factory;
		_pool.SetParent(this.transform);
		_pool.Prewarm(_initialPoolSize);
		List<ParticleSystem> particles = _pool.Request(2) as List<ParticleSystem>;
		foreach (ParticleSystem particle in particles)
		{
			StartCoroutine(DoParticleBehaviour(particle));
		}
		yield return new WaitForSeconds(2);
		_pool.SetParent(null);
		yield return new WaitForSeconds(2);
		_pool.SetParent(this.transform);
	}

	private IEnumerator DoParticleBehaviour(ParticleSystem particle)
	{
		particle.transform.position = Random.insideUnitSphere * 5f;
		particle.Play();
		yield return new WaitForSeconds(particle.main.duration);
		particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		yield return new WaitUntil(() => particle.particleCount == 0);
		_pool.Return(particle);
	}

}
