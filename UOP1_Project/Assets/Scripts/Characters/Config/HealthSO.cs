using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersHealth", menuName = "EntityConfig/Player's Health")]
public class HealthSO : ScriptableObject
{
	[Tooltip("Initial health")]
	[SerializeField] private int _maxHealth = default;
	[Tooltip("current health")]
	[SerializeField] private int _currentHealth = default;

	public int MaxHealth => _maxHealth;
	public int CurrentHealth => _currentHealth;
	public void SetMaxHealth(int newValue)
	{
		_maxHealth = newValue;
	}

	public void SetCurrentHealth(int newValue)
	{
		_currentHealth = newValue;
	}
	public void InflictDamage(int DamageValue)
	{
		_currentHealth -= DamageValue;
	}
	public void RestoreHealth(int HealthValue)
	{
		_currentHealth += HealthValue;
	}
}
