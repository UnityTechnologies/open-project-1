using UnityEngine;
using OP1.Factory;

[CreateAssetMenu(fileName = "New Particle Factory", menuName = "Factory/Particle Factory")]
public class ParticleFactory : ComponentFactorySO<PoolableParticle>
{
	[SerializeField]
	PoolableParticle _prefab = default;
	public override PoolableParticle Prefab => _prefab;
}

