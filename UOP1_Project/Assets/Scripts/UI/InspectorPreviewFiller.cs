using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectorPreviewFiller : MonoBehaviour
{
	[SerializeField]
	private Image _previewImage = default;


	public void FillPreview(Item ItemToInspect)
	{

		_previewImage.gameObject.SetActive(true);
		_previewImage.sprite = ItemToInspect.PreviewImage;

	}



}
