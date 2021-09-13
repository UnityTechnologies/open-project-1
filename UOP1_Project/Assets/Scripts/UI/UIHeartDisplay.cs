using UnityEngine;
using UnityEngine.UI;

public class UIHeartDisplay : MonoBehaviour
{
	[SerializeField] Image _slidingImage = default;
	[SerializeField] Image _combatBackgroundImage = default;
	[SerializeField] Image _backgroundImage = default;

	[Header("Listening on")]
	[SerializeField] BoolEventChannelSO _combatStateEvent = default;

	private void OnEnable()
	{
		_combatStateEvent.OnEventRaised += OnCombatState;
	}

	private void OnDisable()
	{
		_combatStateEvent.OnEventRaised -= OnCombatState;
	}

	public void SetImage(float percent)
	{
		_slidingImage.fillAmount = percent;
		if (percent == 0f)
		{
			_backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 0.5f);
		}
		else
		{
			_backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 1f);
		}
	}

	private void OnCombatState(bool isCombat)
	{
		_combatBackgroundImage.gameObject.SetActive(isCombat);
	}
}
