using UnityEngine;
using OP1.Pool;
using OP1.Factory;

[CreateAssetMenu(fileName = "New Particle Pool", menuName = "Pool/Particle Pool")]
public class ParticlePool : ComponentPool<PoolableParticle>
{
	[SerializeField]
	private ParticleFactory _factory;
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
			_factory = value as ParticleFactory;
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

