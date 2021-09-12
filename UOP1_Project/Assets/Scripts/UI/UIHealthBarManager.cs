using UnityEngine;

public class UIHealthBarManager : MonoBehaviour
{
	[SerializeField] private HealthSO _currentHealth = default;
	[SerializeField] private HealthConfigSO _healthConfig = default;
	[SerializeField] private UIHeartDisplay[] _heartImages = default;

	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO _deathEvent = default;
	[SerializeField] private VoidEventChannelSO _updateHealthEvent = default;

	private void OnEnable()
	{
		_updateHealthEvent.OnEventRaised += UpdateHeartImages;
		_deathEvent.OnEventRaised += UpdateHeartImages;

		SetHealthBar();
	}

	private void OnDestroy()
	{
		_updateHealthEvent.OnEventRaised -= UpdateHeartImages;
		_deathEvent.OnEventRaised -= UpdateHeartImages;
	}

	private void SetHealthBar()
	{
		_currentHealth.SetMaxHealth(_healthConfig.InitialHealth);
		_currentHealth.SetCurrentHealth(_healthConfig.InitialHealth);

		UpdateHeartImages();
	}

	private void UpdateHeartImages()
	{
		int heartValue = _currentHealth.MaxHealth / _heartImages.Length;
		int filledHeartCount = Mathf.FloorToInt((float)_currentHealth.CurrentHealth / heartValue);

		for (int i = 0; i < _heartImages.Length; i++)
		{
			float heartPercent = 0;

			if (i < filledHeartCount)
			{
				heartPercent = 1;
			}
			else if (i == filledHeartCount)
			{
				heartPercent = ((float)_currentHealth.CurrentHealth - (float)filledHeartCount * (float)heartValue) / (float)heartValue;
			}
			else
			{
				heartPercent = 0;
			}
			_heartImages[i].SetImage(heartPercent);
		}
	}
}
