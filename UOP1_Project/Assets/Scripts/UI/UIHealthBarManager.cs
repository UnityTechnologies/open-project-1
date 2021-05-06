using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarManager : MonoBehaviour
{
	Transform target;
	int maxHealth=0;
	int currentHealth=0;
	[SerializeField] private Slider _healthBar = default;

	[Header("Listening to")]
	[SerializeField] private IntEventChannelSO _setHealthBar = default;
	[SerializeField] private IntEventChannelSO _inflictDamage = default;
	[SerializeField] private IntEventChannelSO _restoreHealth = default;

	private void OnEnable()
	{

		if((GetComponent<Canvas>()!=null)&&(Camera.main!=null))
		{
			GetComponent<Canvas>().worldCamera = Camera.main;
			target = Camera.main.transform; 

		}
	}
	private void Start()
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
		setSlider(); 

	}
	public void InflictDamage(int _damage)
	{
		currentHealth -= _damage;
		setSlider();
	}
	public void RestoreHealth(int _healthToAdd)
	{
		currentHealth += _healthToAdd;
		setSlider();
	}

	void setSlider()
	{
		//clamp current value 
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		//find new slider value
		float sliderValue = 0;
		sliderValue = currentHealth / maxHealth;
		_healthBar.value = sliderValue; 

	}

	private void Update()
	{
		if (target != null)
			transform.LookAt(target, Vector3.down);
	}
}
