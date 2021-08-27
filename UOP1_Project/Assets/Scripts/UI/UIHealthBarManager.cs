using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIHealthBarManager : MonoBehaviour
{
	[SerializeField]
	private HealthSO _currentHealth = default;
	[SerializeField]
	private HealthConfigSO _healthConfig = default;


	[SerializeField] private UIHeartDisplay[] _heartImages = default;
	[SerializeField] private TextMeshProUGUI healthText = default;

	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO _deathEvent = default;

	[SerializeField] private VoidEventChannelSO _updateHealthEvent = default;

	private void OnEnable()
	{
		_deathEvent.OnEventRaised += SetHealthBar;
		_updateHealthEvent.OnEventRaised += SetHeartImages;

		_deathEvent.OnEventRaised += RegisterDeath;
		SetHealthBar();
	}
	private void OnDestroy()
	{
		_updateHealthEvent.OnEventRaised -= SetHeartImages;
		_deathEvent.OnEventRaised -= SetHealthBar;
		_deathEvent.OnEventRaised -= RegisterDeath;

	}
	private void Start()
	{
	}
	private void OnLevelWasLoaded(int level)
	{
		SetHeartImages();
	}
	public void SetHealthBar()
	{
		_currentHealth.SetMaxHealth(_healthConfig.MaxHealth);
		_currentHealth.SetCurrentHealth(_healthConfig.MaxHealth);

		SetHeartImages();

	}
	public void InflictDamage(int _damage)
	{
		SetHeartImages();
	}
	public void RestoreHealth(int _healthToAdd)
	{
		SetHeartImages();
	}
	public void RegisterDeath()
	{
		SetHealthBar();

	}
	void SetHeartImages()
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
