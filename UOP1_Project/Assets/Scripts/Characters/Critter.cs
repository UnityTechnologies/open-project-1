using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Critter : MonoBehaviour
{
	[SerializeField] private int _fullHealth = 20;
	[SerializeField] private float _waitTime = default;
	[SerializeField] private float _speed = default;
	[SerializeField] private float _roamingDistance = default;
	[SerializeField] private TransformAnchor playerTransform = default;

	private int _currentHealth = default;
	private float _currentWaitTime = default;
	private Vector3 _startPosition = default;
	private Vector3 _roamingPosTarget = default;
	private NavMeshAgent _agent;

	public bool isPlayerInAlertZone { get; set; }
	public bool isPlayerInAttackZone { get; set; }
	public bool getHit { get; set; }
	public bool isDead { get; set; }
	public bool isRoaming { get; set; }


	private void Awake()
	{
		_currentHealth = _fullHealth;
		_startPosition = transform.position;
		_currentWaitTime = _waitTime;
		_agent = GetComponent<NavMeshAgent>();
		_roamingPosTarget = GetRoamingPosition();
		if (_agent != null)
		{
			_agent.speed = _speed;
		}
	}

	private void Update()
	{
		// Play roaming and chasing movement only if the critter is not static.
		if (_speed > 0)
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
				_agent.SetDestination(playerTransform.Transform.position);
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
				_agent.SetDestination(_roamingPosTarget);
				if (!_agent.hasPath)
				{
					_currentWaitTime -= Time.deltaTime;
					// Have a short rest at destination before roaming somewhere else.
					if (_currentWaitTime < 0)
					{
						_roamingPosTarget = GetRoamingPosition();
						_currentWaitTime = _waitTime;
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
		return _startPosition + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized * Random.Range(_roamingDistance / 2, _roamingDistance);
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

	public void DestroyCritter()
	{
		GameObject.Destroy(this.gameObject);
	}
}
