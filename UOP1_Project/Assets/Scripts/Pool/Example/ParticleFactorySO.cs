using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewParticleFactory", menuName = "Factory/Particle Factory")]
public class ParticleFactorySO : FactorySO<ParticleSystem>
{
	public override ParticleSystem Create()
	{
		return new GameObject("ParticleSystem").AddComponent<ParticleSystem>();
	}
}

