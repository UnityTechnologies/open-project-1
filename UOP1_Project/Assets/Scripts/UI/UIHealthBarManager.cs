using UnityEngine;

public class UIHealthBarManager : MonoBehaviour
{
	[SerializeField] private HealthSO _protagonistHealth = default; //the HealthBar is watching this object, which is the health of the player
	[SerializeField] private HealthConfigSO _healthConfig = default;
	[SerializeField] private UIHeartDisplay[] _heartImages = default;

	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO _UIUpdateNeeded = default; //The player's Damageable issues this

	private void OnEnable()
	{
		_UIUpdateNeeded.OnEventRaised += UpdateHeartImages;
		
		InitializeHealthBar();
	}

	private void OnDestroy()
	{
		_UIUpdateNeeded.OnEventRaised -= UpdateHeartImages;
	}

	private void InitializeHealthBar()
	{
		_protagonistHealth.SetMaxHealth(_healthConfig.InitialHealth);
		_protagonistHealth.SetCurrentHealth(_healthConfig.InitialHealth);

		UpdateHeartImages();
	}

	private void UpdateHeartImages()
	{
		int heartValue = _protagonistHealth.MaxHealth / _heartImages.Length;
		int filledHeartCount = Mathf.FloorToInt((float)_protagonistHealth.CurrentHealth / heartValue);

		for (int i = 0; i < _heartImages.Length; i++)
		{
			float heartPercent = 0;

			if (i < filledHeartCount)
			{
				heartPercent = 1;
			}
			else if (i == filledHeartCount)
			{
				heartPercent = ((float)_protagonistHealth.CurrentHealth - (float)filledHeartCount * (float)heartValue) / (float)heartValue;
			}
			else
			{
				heartPercent = 0;
			}
			_heartImages[i].SetImage(heartPercent);
		}
	}
}
