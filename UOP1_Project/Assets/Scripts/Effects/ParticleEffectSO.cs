using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ParticleEffect", menuName = "Effects/Create Particle Effect")]
public class ParticleEffectSO : ScriptableObject
{
	[SerializeField] private ParticleEffectPoolSO _pool;

	public ParticleEffectPoolSO Pool
	{
		get
		{
			return _pool;
		}
	}

}
