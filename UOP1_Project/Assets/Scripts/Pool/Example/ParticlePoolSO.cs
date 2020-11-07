using UnityEngine;
using UOP1.Pool;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewParticlePool", menuName = "Pool/Particle Pool")]
public class ParticlePoolSO : ComponentPoolSO<PoolableParticle>
{
	[SerializeField]
	private ParticleFactorySO _factory;
	[SerializeField]
	private int _initialPoolSize;

	public override IFactory<PoolableParticle> Factory
	{
		get
		{
			return _factory;
		}
		set
		{
			_factory = value as ParticleFactorySO;
		}
	}

	public override int InitialPoolSize
	{
		get
		{
			return _initialPoolSize;
		}
		set
		{
			_initialPoolSize = value;
		}
	}
}

