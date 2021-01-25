using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Critter : MonoBehaviour
{
	[SerializeField]
	private CritterConfigSO _critterSO;

	[SerializeField]
	private AttackConfigSO _attackConfigSO;

	[SerializeField]
	private ChasingConfigSO _chasingConfigSO;

	[SerializeField]
	private RoamingAroundSpawningPositionConfigSO _roamingAroundSpawningPositionConfigSO;

	[SerializeField]
	private DroppableRewardConfigSO _droppableRewardSO;

	[SerializeField]
	private GetHitEffectConfigSO _getHitEffectSO;

	[SerializeField]
	private Renderer _mainMeshRenderer;

	private int _currentHealth = default;

	public bool isPlayerInAlertZone { get; set; }
	public bool isPlayerInAttackZone { get; set; }
	public bool getHit { get; set; }
	public bool isDead { get; set; }

	public RoamingAroundSpawningPositionConfigSO RoamingAroundSpawningPositionConfig => _roamingAroundSpawningPositionConfigSO;
	public DroppableRewardConfigSO DropableRewardConfig => _droppableRewardSO;
	public GetHitEffectConfigSO GetHitEffectConfig => _getHitEffectSO;
	public Renderer MainMeshRenderer => _mainMeshRenderer;
	public ChasingConfigSO ChasingConfig => _chasingConfigSO;
	public AttackConfigSO AttackConfigSO => _attackConfigSO;
	
	private void Awake()
	{
		_currentHealth = _critterSO.MaxHealth;
	}

	private void ReceiveAnAttack(int damange)
	{
		_currentHealth -= damange;
		getHit = true;
		if (_currentHealth <= 0)
		{
			isDead = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Weapon playerWeapon = other.GetComponent<Weapon>();
		if (!getHit && playerWeapon != null && playerWeapon.Enable)
		{
			ReceiveAnAttack(playerWeapon.AttackStrength);
		}
	}
}
