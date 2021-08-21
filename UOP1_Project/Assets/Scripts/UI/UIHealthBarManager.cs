using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIHealthBarManager : MonoBehaviour
{
	int _maxHealth;
	float _currentHealth;
	[SerializeField] private UIHeartDisplay[] _heartImages = default;
	[SerializeField] private TextMeshProUGUI healthText = default;

	[Header("Listening to")]
	[SerializeField] private IntEventChannelSO _setHealthBar = default;
	[SerializeField] private IntEventChannelSO _inflictDamage = default;
	[SerializeField] private IntEventChannelSO _restoreHealth = default;


	private void OnEnable()
	{

		_setHealthBar.OnEventRaised += SetHealthBar;

		_inflictDamage.OnEventRaised += InflictDamage;

		_restoreHealth.OnEventRaised += RestoreHealth;

	}
	private void OnDestroy()
	{

		_setHealthBar.OnEventRaised -= SetHealthBar;

		_inflictDamage.OnEventRaised -= InflictDamage;

		_restoreHealth.OnEventRaised -= RestoreHealth;

	}
	public void SetHealthBar(int _maxHealth)
	{
		this._maxHealth = _maxHealth;
		_currentHealth = _maxHealth;
		SetHeartImages();

	}
	public void InflictDamage(int _damage)
	{
		_currentHealth -= _damage;
		SetHeartImages();
	}
	public void RestoreHealth(int _healthToAdd)
	{
		_currentHealth += _healthToAdd;
		SetHeartImages();
	}

	void SetHeartImages()
	{

		int heartValue = _maxHealth / _heartImages.Length;
		int filledHeartCount = Mathf.FloorToInt(_currentHealth / heartValue);

		for (int i = 0; i < _heartImages.Length; i++)
		{
			float heartPercent = 0;

			if (i < filledHeartCount)
			{
				heartPercent = 1;
			}
			else if (i == filledHeartCount)
			{
				heartPercent = ((float)_currentHealth - (float)filledHeartCount * (float)heartValue) / (float)heartValue;
			}
			else
			{
				heartPercent = 0;
			}
			_heartImages[i].SetImage(heartPercent);

		}

	}



}
