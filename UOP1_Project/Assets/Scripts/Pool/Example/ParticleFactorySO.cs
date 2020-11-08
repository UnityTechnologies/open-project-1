using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewParticleFactory", menuName = "Factory/Particle Factory")]
public class ParticleFactorySO : ComponentFactorySO<PoolableParticle>
{
	[SerializeField]
	private PoolableParticle _prefab = default;

	public override PoolableParticle Prefab
	{
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

