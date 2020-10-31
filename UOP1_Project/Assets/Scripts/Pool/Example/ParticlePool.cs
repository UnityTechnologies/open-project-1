using OP1.Factory;
using OP1.Pool;
using UnityEngine;

[CreateAssetMenu(fileName ="New Particle Pool",menuName ="Pool/Particle Pool")]
public class ParticlePool : ComponentPoolSO<PoolableParticle>
{
	[SerializeField]
	private ParticleFactory _factory = default;
	public override IFactory<PoolableParticle> Factory => _factory;

}
