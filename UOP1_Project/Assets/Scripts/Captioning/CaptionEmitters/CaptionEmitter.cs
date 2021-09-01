using TMPro;
using UnityEngine;

namespace Assets.Scripts.Captioning.CaptionEmitters
{
	public class CaptionEmitter : MonoBehaviour
	{
		public void Display(Caption caption, Vector3 position = default)
		{
			transform.position = position;
			var captionTextComponent = GetComponentInChildren<TextMeshPro>();

			if (!string.IsNullOrEmpty(caption.SoundText.TableReference))
			{
				captionTextComponent.text = caption.SoundText.GetLocalizedString();
			}
		}
	}
}
