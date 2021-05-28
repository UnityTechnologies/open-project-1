using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealthBarManager : MonoBehaviour
{
	Transform target;
	int maxHealth=0;
	int currentHealth=0;
	[SerializeField] private Image[] _heartImages = default;
	[SerializeField] private Text healthText = default;

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
		maxHealth = _maxHealth;
		currentHealth = _maxHealth;
		setHeartImages(); 

	}
	public void InflictDamage(int _damage)
	{
		currentHealth -= _damage;
		setHeartImages();
	}
	public void RestoreHealth(int _healthToAdd)
	{
		currentHealth += _healthToAdd;
		setHeartImages();
	}

	void setHeartImages()
	{
		//clamp current value 
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		healthText.text = currentHealth + "/" + maxHealth; 
		//find max heart index
		int heartIndex = Mathf.CeilToInt( ((float)currentHealth / (float)maxHealth) * _heartImages.Length);
	
		for (int i = 0; i < _heartImages.Length; i++)
		{
			_heartImages[i].color = Color.red;
			if (_heartImages.Length > heartIndex)
			{
				_heartImages[i].gameObject.SetActive(i <= heartIndex);
			}
		}
		
	}


	
}
