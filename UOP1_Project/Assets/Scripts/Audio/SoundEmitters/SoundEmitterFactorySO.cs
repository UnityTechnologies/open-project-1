using UnityEngine;
using UOP1.Factory;

[CreateAssetMenu(fileName = "NewSoundEmitterFactory", menuName = "Factory/SoundEmitter Factory")]
public class SoundEmitterFactorySO : ComponentFactorySO<SoundEmitter>
{
	[SerializeField]
	private SoundEmitter _prefab = default;

	public override SoundEmitter Prefab
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
