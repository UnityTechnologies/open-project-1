using UnityEngine;
using UOP1.Factory;

namespace Assets.Scripts.Captioning.CaptionEmitters
{
	[CreateAssetMenu(fileName = "NewCaptionEmitterFactory", menuName = "Factory/CaptionEmitter Factory")]
	public class CaptionEmitterFactorySO : FactorySO<CaptionEmitter>
	{
		public CaptionEmitter prefab = default;

		public override CaptionEmitter Create()
		{
			return Instantiate(prefab);
		}
	}
}
