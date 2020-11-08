using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTester : MonoBehaviour
{
	[SerializeField]
	private ParticlePoolSO _pool = default;

	private IEnumerator Start()
	{
		List<PoolableParticle> particles = _pool.Request(10) as List<PoolableParticle>;
		foreach (PoolableParticle particle in particles)
		{
			particle.transform.position = Random.insideUnitSphere * 5f;
			particle.Play();
		}
		yield return new WaitForSeconds(5f);
		_pool.Return(particles);
	}
}
