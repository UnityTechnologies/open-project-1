using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class ClosedCaptioningSystem : MonoBehaviour
	{
		public GameObject CaptionPrefab;

		public void VisualiseAudioClip(Caption caption, Vector3 position = default)
		{
			var newCaption = Instantiate(CaptionPrefab, position, Quaternion.identity);
			var captionTextComponent = newCaption.GetComponentInChildren<TextMeshPro>();

			if (!string.IsNullOrEmpty(caption.SoundText.TableReference))
			{
				captionTextComponent.text = caption.SoundText.GetLocalizedString();
			}
			StartCoroutine(DestroyNewCaption(newCaption, caption.Duration));
		}

		IEnumerator DestroyNewCaption(GameObject newOnomatopoeia, float duration)
		{
			yield return new WaitForSeconds(duration);
			Destroy(newOnomatopoeia);
		}
	}
}
