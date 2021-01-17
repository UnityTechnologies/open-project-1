using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour
{
	[SerializeField]
	private CritterSO _critterSO;

	[SerializeField]
	private GameObject _collectibleItemPrefab;

	private int _currentHealth = default;

	public bool isPlayerInAlertZone { get; set; }
	public bool isPlayerInAttackZone { get; set; }
	public bool getHit { get; set; }
	public bool isDead { get; set; }

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
