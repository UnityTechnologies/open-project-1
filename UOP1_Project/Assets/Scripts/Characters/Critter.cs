using UnityEngine;

public class Critter : DroppingEntity
{
	[SerializeField]
	private AttackConfigSO _attackConfigSO;

	[SerializeField]
	private ChasingConfigSO _chasingConfigSO;

	public bool isPlayerInAlertZone { get; set; }
	public bool isPlayerInAttackZone { get; set; }

	public ChasingConfigSO ChasingConfig => _chasingConfigSO;
	public AttackConfigSO AttackConfigSO => _attackConfigSO;
	
}
