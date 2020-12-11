using UnityEngine;
using UOP1.Pool;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewParticleEffectPool", menuName = "Pool/Particle Effect Pool")]
public class ParticleEffectPoolSO : ComponentPoolSO<ParticleSystem>
{
	[SerializeField]
	private ParticleEffectFactorySO _factory;

	public override IFactory<ParticleSystem> Factory
	{
		get
		{
			return _factory;
		}
		set
		{
			_factory = value as ParticleEffectFactorySO;
		}
	}
}
