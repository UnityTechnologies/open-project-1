using UnityEngine;
using UnityEngine.UI;

public class UIInspectorPreview : MonoBehaviour
{
	[SerializeField] private Image _previewImage = default;

	public void FillPreview(ItemSO ItemToInspect)
	{
		_previewImage.gameObject.SetActive(true);
		_previewImage.sprite = ItemToInspect.PreviewImage;
	}
}
