using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "CritterVariant", menuName = "Critter/Critter Variant")]
public class CritterSO : ScriptableObject
{
	[Tooltip("The name of the critter")]
	[SerializeField] private LocalizedString _name;

	[Tooltip("Initial critter health")]
	[SerializeField] private int _maxHealth;

	public LocalizedString Name => _name;
	public int MaxHealth => _maxHealth;
}
