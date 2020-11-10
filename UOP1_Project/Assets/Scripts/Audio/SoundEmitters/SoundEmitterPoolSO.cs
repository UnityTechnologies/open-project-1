using UnityEngine;
using System.Linq;
using UOP1.Pool;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewSoundEmitterPool", menuName = "Pool/SoundEmitter Pool")]
public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
{
	[SerializeField]
	private SoundEmitterFactorySO _factory;
	[SerializeField]
	private int _initialPoolSize;

	public override IFactory<SoundEmitter> Factory
	{
		get
		{
			return _factory;
		}
		set
		{
			_factory = value as SoundEmitterFactorySO;
		}
	}

	public override int InitialPoolSize
	{
		get
		{
			return _initialPoolSize;
		}
		set
		{
			_initialPoolSize = value;
		}
	}
}
