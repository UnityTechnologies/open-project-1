using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class ClosedCaptioningSystem : MonoBehaviour
	{
		public GameObject OnomatopoeiaPrefab;

		public void VisualiseAudioClip(Onomatopoeia onomatopoeia, Vector3 position = default)
		{
			var newOnomatopoeia = Instantiate(OnomatopoeiaPrefab, position, Quaternion.identity);
			var onomatopoeiaTextComponent = newOnomatopoeia.GetComponentInChildren<TextMeshPro>();

			if (!string.IsNullOrEmpty(onomatopoeia.SoundText.TableReference))
			{
				onomatopoeiaTextComponent.text = onomatopoeia.SoundText.GetLocalizedString();
			}
			StartCoroutine(DestroyNewOnomatopoeia(newOnomatopoeia, onomatopoeia.Duration));
		}

		IEnumerator DestroyNewOnomatopoeia(GameObject newOnomatopoeia, float duration)
		{
			yield return new WaitForSeconds(duration);
			Destroy(newOnomatopoeia);
		}
	}
}
