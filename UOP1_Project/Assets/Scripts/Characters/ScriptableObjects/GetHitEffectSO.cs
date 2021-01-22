using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "GetHitEffectSO", menuName = "Characters/Get Hit Effect")]
public class GetHitEffectSO : ScriptableObject
{
	[Tooltip("Flashing effect color applied when getting hit.")]
	[SerializeField]
	private Color _getHitFlashingColor = default;

	[Tooltip("Flashing effect duration (in seconds).")]
	[SerializeField]
	private float _getHitFlashingDuration = 0.5f;

	[Tooltip("Flashing effect speed (number of flashings during the duration).")]
	[SerializeField]
	private float _getHitFlashingSpeed = 3.0f;

	public GetHitData CreateData(Material material)
	{
		GetHitData data = new GetHitData();

		data.material = material;
		data.getHitTintingDuration = _getHitFlashingDuration;
		data.baseTintColor = material.GetColor("_MainColor");
		data.innerFlashingTime = 0.0f;

		return data;
	}

	public void ApplyHitEffectIfNeeded(GetHitData data)
	{
		if (data.effectRunning)
		{
			if (data.innerFlashingTime > 0)
			{
				Color tintingColor = computeGetHitTintingColor(data);
				data.material.SetColor("_MainColor", tintingColor);
				data.innerFlashingTime -= Time.deltaTime;
			}
			else
			{
				data.material.SetColor("_MainColor", data.baseTintColor);
				data.effectRunning = false;
			}
		}
		
	}

	private Color computeGetHitTintingColor(GetHitData data)
	{
		Color finalTintingColor = Color.Lerp(data.baseTintColor, _getHitFlashingColor, _getHitFlashingColor.a);
		float tintingTiming = (_getHitFlashingDuration - data.innerFlashingTime) * _getHitFlashingSpeed / _getHitFlashingDuration;
		return Color.Lerp(data.baseTintColor, finalTintingColor, (-Mathf.Cos(Mathf.PI * 2 * tintingTiming) + 1) / 2);
	}
}

public class GetHitData
{
	internal Material material;
	internal float getHitTintingDuration;
	internal float innerFlashingTime;
	internal Color baseTintColor;
	internal bool effectRunning;

	public void GetHit()
	{
		innerFlashingTime = getHitTintingDuration;
		effectRunning = true;
	}
}
