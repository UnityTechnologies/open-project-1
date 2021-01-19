using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Critter : MonoBehaviour
{
	[SerializeField]
	private CritterSO _critterSO;

	[SerializeField]
	private GameObject _collectibleItemPrefab;

	private int _currentHealth = default;
	private float _currentWaitTime = default;
	private Vector3 _startPosition = default;
	private Vector3 _roamingPosTarget = default;
	private NavMeshAgent _agent;
	private bool _agentActiveOnNavMesh = default;

	public bool isPlayerInAlertZone { get; set; }
	public bool isPlayerInAttackZone { get; set; }
	public bool getHit { get; set; }
	public bool isDead { get; set; }
	public bool isRoaming { get; set; }


	private void Awake()
	{
		_currentHealth = _critterSO.MaxHealth;
		_startPosition = transform.position;
		_currentWaitTime = _critterSO.WaitTime;
		_agent = GetComponent<NavMeshAgent>();
		_agentActiveOnNavMesh = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;
		_roamingPosTarget = GetRoamingPosition();
	}

	private void Update()
	{
		// Play roaming and chasing movement only if the critter is not static.
		if (_agentActiveOnNavMesh)
		{
			isRoaming = false;
			// Critter fainting is not moving.
			if (isDead)
			{
				_agent.isStopped = true;
			}
			// Chasing the player nearby
			else if (isPlayerInAlertZone)
			{
				_agent.speed = _critterSO.ChasingSpeed;
				_agent.SetDestination(_critterSO.PlayerPosition);
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
				_agent.speed = _critterSO.RoamingSpeed;
				_agent.SetDestination(_roamingPosTarget);
				if (!_agent.hasPath)
				{
					_currentWaitTime -= Time.deltaTime;
					// Have a short rest at destination before roaming somewhere else.
					if (_currentWaitTime < 0)
					{
						_roamingPosTarget = GetRoamingPosition();
						_currentWaitTime = _critterSO.WaitTime;
					}
				}
				else
				{
					isRoaming = true;
				}
			}
		}
	}

	// Compute a random target position around the starting position.
	private Vector3 GetRoamingPosition()
	{
		return _startPosition + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized * Random.Range(_critterSO.RoamingDistance / 2, _critterSO.RoamingDistance);
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

	public void CritterIsDead()
	{
		// Drop items
		for (int i = 0; i < _critterSO.GetNbDroppedItems(); i++)
		{
			Item item = _critterSO.GetDroppedItem();

			float randPosRight = Random.value * 2 - 1.0f;
			float randPosForward = Random.value * 2 - 1.0f;

			GameObject collectibleItem = GameObject.Instantiate(_collectibleItemPrefab,
				gameObject.transform.position + _collectibleItemPrefab.transform.localPosition +
				2 * (randPosForward * Vector3.forward + randPosRight * Vector3.right),
				gameObject.transform.localRotation);
			collectibleItem.GetComponent<CollectibleItem>().CurrentItem = item;
		}

		// Remove Critter from the game
		GameObject.Destroy(this.gameObject);
	}
}
