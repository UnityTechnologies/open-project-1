using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
	[SerializeField] private AttackConfigSO _attackConfigSO;
	[SerializeField] [Tooltip("Any additional events to be called upon a successful hit")]
	private UnityEvent onHit;

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
				{
					damageableComp.ReceiveAnAttack(_attackConfigSO.AttackStrength);
					// Invoke any necessary hit events
					onHit?.Invoke();
				}
			}
		}
	}
}
