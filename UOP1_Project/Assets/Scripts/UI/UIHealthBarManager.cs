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
	[SerializeField] private IntEventChannelSO _inflictDamage = default;
	[SerializeField] private IntEventChannelSO _restoreHealth = default;
	[SerializeField] private VoidEventChannelSO _deathEvent = default;


	private void OnEnable()
	{
		_deathEvent.OnEventRaised += SetHealthBar;
		_inflictDamage.OnEventRaised += InflictDamage;

		_restoreHealth.OnEventRaised += RestoreHealth;
		_deathEvent.OnEventRaised += RegisterDeath;
		SetHealthBar();
	}
	private void OnDestroy()
	{
		_inflictDamage.OnEventRaised -= InflictDamage;

		_deathEvent.OnEventRaised -= SetHealthBar;
		_restoreHealth.OnEventRaised -= RestoreHealth;
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
		_currentHealth.MaxHealth = _healthConfig.MaxHealth;
		_currentHealth.CurrentHealth = _healthConfig.MaxHealth;

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
