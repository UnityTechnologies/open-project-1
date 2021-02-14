using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	[SerializeField] private AttackConfigSO _attackConfigSO;

	public AttackConfigSO AttackConfig => _attackConfigSO;

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		// Avoid friendly fire!
		if (!other.CompareTag(gameObject.tag))
		{
			if (other.TryGetComponent(out Damageable damageableComp))
			{
				if (!damageableComp.GetHit)
					damageableComp.ReceiveAnAttack(_attackConfigSO.AttackStrength);
			}
		}
	}
}
