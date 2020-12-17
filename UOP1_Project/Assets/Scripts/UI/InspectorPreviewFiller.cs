using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectorPreviewFiller : MonoBehaviour
{
	[SerializeField]
	private Image previewImage;


	public void FillPreview(Item ItemToInspect)
	{

		previewImage.gameObject.SetActive(true);
		previewImage.sprite = ItemToInspect.PreviewImage;

	}



}
