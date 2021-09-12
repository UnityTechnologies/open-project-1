using UnityEngine;
using UOP1.Factory;
using UOP1.Pool;

namespace Assets.Scripts.Captioning.CaptionEmitters
{
	[CreateAssetMenu(fileName = "NewCaptionEmitterPool", menuName = "Pool/CaptionEmitter Pool")]
	public class CaptionEmitterPoolSO : ComponentPoolSO<CaptionEmitter>
	{
		[SerializeField]
		private CaptionEmitterFactorySO _factory;

		public override IFactory<CaptionEmitter> Factory
		{
			get
			{
				return _factory;
			}
			set
			{
				_factory = value as CaptionEmitterFactorySO;
			}
		}
	}

}
