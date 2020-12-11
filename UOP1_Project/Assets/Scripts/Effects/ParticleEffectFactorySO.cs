using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewParticleEffectFactory", menuName = "Factory/Particle Effect Factory")]
public class ParticleEffectFactorySO : FactorySO<ParticleSystem>
{
	[SerializeField]
	private ParticleSystem _prefab;

	public override ParticleSystem Create()
	{
		return Instantiate(_prefab);
	}
}
