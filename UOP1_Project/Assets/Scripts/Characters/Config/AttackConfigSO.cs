using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "EntityConfig/Attack Config")]
public class AttackConfigSO : ScriptableObject
{
	[Tooltip("Character attack strength")]
	[SerializeField] private int _attackStrength;

	[Tooltip("Character attack reload duration (in second).")]
	[SerializeField] private float _attackReloadDuration;

	public int AttackStrength => _attackStrength;
	public float AttackReloadDuration => _attackReloadDuration;
}
