using UnityEngine;
using UOP1.Pool;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewParticlePool", menuName = "Pool/Particle Pool")]
public class ParticlePoolSO : ComponentPoolSO<ParticleSystem>
{
	[SerializeField]
	private ParticleFactorySO _factory;

	public override IFactory<ParticleSystem> Factory
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
}

