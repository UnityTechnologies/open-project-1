using Assets.Scripts.Captioning.OffscreenIndicators;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Captioning.CaptionEmitters
{
	public class CaptionEmitter : MonoBehaviour
	{
		[SerializeField] private TargetIndicator _offscreeenTargetIndicator;
		public void Display(Caption caption, Vector3 position = default)
		{
			transform.position = position;
			var captionTextComponent = GetComponentInChildren<TextMeshPro>();

			if (!string.IsNullOrEmpty(caption.SoundText.TableReference))
			{
				captionTextComponent.text = caption.SoundText.GetLocalizedString();
			}
		}

		public void ActivateOffscreenIndicator(bool active)
		{
			_offscreeenTargetIndicator.gameObject.SetActive(active);
		}

		public TargetIndicator GetOffscreentTargetIndicator()
		{
			return _offscreeenTargetIndicator;
		}
	}
}
