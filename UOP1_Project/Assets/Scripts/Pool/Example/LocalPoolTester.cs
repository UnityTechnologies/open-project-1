using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPoolTester : MonoBehaviour
{
	[SerializeField]
	private PoolableParticle _prefab = default;
	[SerializeField]
	private int _initialPoolSize = 5;

	private ParticlePoolSO _pool;
	private ParticleFactorySO _factory;

	private IEnumerator Start()
	{
		_factory = ScriptableObject.CreateInstance<ParticleFactorySO>();
		_factory.Prefab = _prefab;
		_pool = ScriptableObject.CreateInstance<ParticlePoolSO>();
		_pool.name = gameObject.name;
		_pool.Factory = _factory;
		_pool.InitialPoolSize = _initialPoolSize;
		List<PoolableParticle> particles = _pool.Request(10) as List<PoolableParticle>;
		foreach (PoolableParticle particle in particles)
		{
			particle.transform.position = Random.insideUnitSphere * 5f;
			particle.Play();
		}
		yield return new WaitForSecondsRealtime(5f);
		_pool.Return(particles);
	}

}
