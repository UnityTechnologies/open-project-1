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

	public ParticleSystem Request()
	{
		ParticleSystem particle = base.Request();
		// Reset Transform of the particle.
		particle.transform.localPosition = Vector3.zero;
		particle.transform.localRotation = Quaternion.identity;
		particle.transform.localScale = Vector3.one;
		return particle;
	}

	public ParticleSystem Request(Transform parent)
	{
		ParticleSystem particle = base.Request();
		particle.transform.SetParent(parent);
		// Reset Transform of the particle.
		particle.transform.localPosition = Vector3.zero;
		particle.transform.localRotation = Quaternion.identity;
		particle.transform.localScale = Vector3.one;
		return particle;
	}
}
