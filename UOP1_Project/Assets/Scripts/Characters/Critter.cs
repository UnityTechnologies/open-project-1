using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour
{
	[SerializeField] private int _fullHealth = 20;

	private int _currentHealth = default;
	private bool _playerInAlertZone = default;

	public bool IsPlayerInAlertZone
	{
		get => _playerInAlertZone;
		set => _playerInAlertZone = value;
	}

	private bool _isPlayerInAttackZone = default;

	public bool IsPlayerInAttackZone
	{
		get => _isPlayerInAttackZone;
		set => _isPlayerInAttackZone = value;
	}

	private bool _getHit = default;
	public bool GetHit
	{
		get => _getHit;
		set => _getHit = value;
	}

	private bool _isDead = default;
	public bool IsDead
	{
		get => _isDead;
		set => _isDead = true;
	}

	private void Awake()
	{
		_currentHealth = _fullHealth;
	}

	private void ReceiveAnAttack(int damange)
	{
		_currentHealth -= damange;
		_getHit = true;
		if (_currentHealth <= 0)
		{
			_isDead = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Weapon playerWeapon = other.GetComponent<Weapon>();
		if (!_getHit && playerWeapon != null && playerWeapon.Enable)
		{
			ReceiveAnAttack(playerWeapon.AttackStrength);
		}
	}

	public void DestroyCritter()
	{
		GameObject.Destroy(this.gameObject);
	}
}
