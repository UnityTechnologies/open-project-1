using UnityEngine;
using UnityEngine.AI;

public class Critter : MonoBehaviour, IAttackerCharacter
{
	//-----------------------------------
	// Critter behaviour described through the listed SO

	[SerializeField]
	private CritterSO _critterSO;

	[SerializeField]
	private AttackConfigSO _attackConfigSO;
	private AttackData _attackData;

	[SerializeField]
	private ChasingConfigSO _chasingConfigSO;

	[SerializeField]
	private NpcMovementSO _npcMovementSO;
	private NpcMovementData _npcMovementData;

	[SerializeField]
	private DroppableRewardSO _droppableRewardSO;

	[SerializeField]
	private GetHitEffectSO _getHitEffectSO;
	private GetHitData _getHitData;

	//-----------------------------------

	private NavMeshAgent _agent;
	private bool _agentActiveOnNavMesh = default;

	private int _currentHealth = default;

	[SerializeField]
	private Renderer _renderer;

	// Critter status properties
	public bool isPlayerInAlertZone { get; set; }
	public bool isPlayerInAttackZone { get; set; }
	public bool getHit { get; set; }
	public bool isDead { get; set; }
	public bool isRoaming { get; set; }

	public AttackConfigSO getAttackConfig()
	{
		return _attackConfigSO;
	}

	public void TriggerAttack()
	{
		_attackConfigSO.Attack(_attackData);
	}

	public bool IsReadyToAttack()
	{
		return _attackConfigSO.isReadyToAttack(_attackData);
	}

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_agentActiveOnNavMesh = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;

		_npcMovementData = _npcMovementSO.CreateNpcMovementData(gameObject);
		_getHitData = _getHitEffectSO.CreateData(_renderer.materials[_renderer.materials.Length - 1]);
		_attackData = _attackConfigSO.createAttackData();

		_currentHealth = _critterSO.MaxHealth;
	}

	private void Update()
	{
		// Play roaming and chasing movement only if the critter is not static.
		if (_agentActiveOnNavMesh)
		{
			isRoaming = false;
			// A fainting critter is not moving.
			if (isDead)
			{
				_agent.isStopped = true;
			}
			else
			{
				if (isPlayerInAlertZone)
				{
					_chasingConfigSO.ChasingTarget(_agent);
					// Stop the critter when close enough to perform an attack.
					if (isPlayerInAttackZone)
					{
						_agent.isStopped = true;
					}
					// Resume critter chasing if the player is moving away from him.
					else
					{
						_agent.isStopped = false;
					}
				}
				// Roaming routine around its start point
				else
				{
					_npcMovementSO.NpcMovementUpdate(_npcMovementData);
					// Pull resulting criter state
					isRoaming = true;
				}
			}
		}

		_getHitEffectSO.ApplyHitEffectIfNeeded(_getHitData);
		_attackConfigSO.computeAttackCapability(_attackData);
	}

	private void OnTriggerEnter(Collider other)
	{
		Weapon playerWeapon = other.GetComponent<Weapon>();
		// If the critter is not already under the GetHit state (safe during this state) and the player weapon is active (only during the player attack state),
		// then the critter receives an attack.
		if (!getHit && playerWeapon != null && playerWeapon.Enable)
		{
			ReceiveAnAttack(playerWeapon.AttackStrength);
		}
	}

	private void ReceiveAnAttack(int damange)
	{
		_currentHealth -= damange;
		getHit = true;
		_getHitData.GetHit();
		if (_currentHealth <= 0)
		{
			isDead = true;
		}
	}

	public void CritterIsDead()
	{
		// Drop Item;
		_droppableRewardSO.DropReward(transform.position);

		// Remove Critter from the game
		GameObject.Destroy(this.gameObject);
	}
}
