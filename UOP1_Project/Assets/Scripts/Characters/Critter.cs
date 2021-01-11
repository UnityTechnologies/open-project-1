using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour
{
	[SerializeField] private int _fullHealth = 20;

	private int _currentHealth = default;

	public bool isPlayerInAlertZone { get; set; }
	public bool isPlayerInAttackZone { get; set; }
	public bool getHit { get; set; }
	public bool isDead { get; set; }

	private void Awake()
	{
		_currentHealth = _fullHealth;
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
