using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersHealth", menuName = "EntityConfig/Player's Health")]
public class HealthSO : ScriptableObject
{
	[Tooltip("Initial health")]
	[SerializeField] private int _maxHealth;
	[Tooltip("current health")]
	[SerializeField] private int _currentHealth;

	public int MaxHealth { get; set; }
	public int CurrentHealth { get; set; }
}
