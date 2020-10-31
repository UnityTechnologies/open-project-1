using OP1.Factory;
using OP1.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTester : MonoBehaviour
{

	[SerializeField]
	private PoolableParticle _prefab = default;
	private Pool<PoolableParticle> _pool;

	private IEnumerator Start()
	{
		_pool = new Pool<PoolableParticle>(new ComponentFactory<PoolableParticle>(_prefab), 5);
		List<PoolableParticle> particles = _pool.Request(10) as List<PoolableParticle>;
		foreach(PoolableParticle particle in particles)
		{
			particle.transform.position = Random.insideUnitSphere * 5f;
			particle.Play();
		}
		yield return new WaitForSecondsRealtime(5f);
		_pool.Return(particles);
	}

}
