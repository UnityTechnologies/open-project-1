using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartDisplay : MonoBehaviour
{
	[SerializeField] Image SlidingImage;
	public void SetImage(float percent)
	{
		SlidingImage.fillAmount = percent;

	}
}
