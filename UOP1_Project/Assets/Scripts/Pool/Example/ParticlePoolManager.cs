using OP1.Pool;
using OP1.Factory;
using UnityEngine;
using System.Collections;

public class ParticlePoolManager : MonoBehaviour
{

	Pool<PoolableParticle> _pool;

	[SerializeField]
	GameObject _particlePrefab = default;
	[Range(.1f,3f)]
	[SerializeField]
	float secondsBetweenSpawn = 1f;

	private void Start()
	{
		_pool = new Pool<PoolableParticle>(new ComponentFactory<PoolableParticle>("ParticlePool",this.gameObject,_particlePrefab),5);
		StartCoroutine(Spawn(secondsBetweenSpawn));
	}

	IEnumerator Spawn(float delay)
	{
		PoolableParticle particle = _pool.Request();
		particle.transform.position = Random.insideUnitSphere * 5f;
		particle.Play();
		_pool.Return(particle);
		yield return new WaitForSecondsRealtime(delay);
		StartCoroutine(Spawn(secondsBetweenSpawn));
	}

}
