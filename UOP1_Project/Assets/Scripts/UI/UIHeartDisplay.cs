using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartDisplay : MonoBehaviour
{
	[SerializeField]
	BoolEventChannelSO _setAggroEvent = default;

	[SerializeField] Image _slidingImage = default;
	[SerializeField] Image _aggroImage = default;
	[SerializeField] Image _bgImage = default;
	private void OnEnable()
	{
		_setAggroEvent.OnEventRaised += SetAggro;
	}
	private void OnDisable()
	{
		_setAggroEvent.OnEventRaised -= SetAggro;
	}
	public void SetImage(float percent)
	{
		_slidingImage.fillAmount = percent;
		if (percent == 0)
		{
			_bgImage.color = new Color(_bgImage.color.r, _bgImage.color.g, _bgImage.color.b, 0.5f);
		}
		else
		{
			_bgImage.color = new Color(_bgImage.color.r, _bgImage.color.g, _bgImage.color.b, 1);
		}

	}
	public void SetAggro(bool isAggro)
	{
		_aggroImage.gameObject.SetActive(isAggro);
	}
}
