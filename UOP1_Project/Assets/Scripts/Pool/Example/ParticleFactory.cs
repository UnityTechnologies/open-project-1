using UnityEngine;
using OP1.Factory;

[CreateAssetMenu(fileName = "New Particle Factory", menuName = "Factory/Particle Factory")]
public class ParticleFactory : ComponentFactory<PoolableParticle>
{
	[SerializeField]
	PoolableParticle _prefab = default;
	public override PoolableParticle Prefab {
		get
		{
			return _prefab;
		}
		set
		{
			_prefab = value;
		}
	}
}

