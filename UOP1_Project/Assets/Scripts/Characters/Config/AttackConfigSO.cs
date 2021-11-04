using UnityEngine;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "EntityConfig/Attack Config")]
public class AttackConfigSO : ScriptableObject
{
	[SerializeField] private int _attackStrength;
	[SerializeField] private float _attackReloadDuration;

	public int AttackStrength => _attackStrength;
	public float AttackReloadDuration => _attackReloadDuration;
}
