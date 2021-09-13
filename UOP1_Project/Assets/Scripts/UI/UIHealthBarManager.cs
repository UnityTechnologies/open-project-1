using UnityEngine;

public class UIHealthBarManager : MonoBehaviour
{
	[SerializeField] private HealthSO _currentHealth = default; //the HealthBar is watching this object, which is the health of the player
	[SerializeField] private HealthConfigSO _healthConfig = default;
	[SerializeField] private UIHeartDisplay[] _heartImages = default;

	[Header("Listening to")]
	[SerializeField] private TransformEventChannelSO _onPlayerInstantiated = default;
	[SerializeField] private VoidEventChannelSO _updateHealthEvent = default;

	private void OnEnable()
	{
		_onPlayerInstantiated.OnEventRaised += ResetHealthOnSpawn;
		_updateHealthEvent.OnEventRaised += UpdateHeartImages;
	}

	private void OnDestroy()
	{
		_onPlayerInstantiated.OnEventRaised -= ResetHealthOnSpawn;
		_updateHealthEvent.OnEventRaised -= UpdateHeartImages;
	}

	private void ResetHealthOnSpawn(Transform t)
	{
		UpdateHeartImages();
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
