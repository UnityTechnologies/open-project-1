using UnityEngine;

public class Attacker : MonoBehaviour
{
	[SerializeField] private Attack _attack;

	private float _innerTime;

	[HideInInspector] public bool IsAttackReloaded = false;

	public void EnableWeapon()
	{
		_attack.gameObject.SetActive(true);
	}

	public void DisableWeapon()
	{
		_attack.gameObject.SetActive(false);
		_innerTime = _attack.AttackConfig.AttackReloadDuration;
	}

	private void Update()
	{
		if (_innerTime > 0)
		{
			_innerTime -= Time.deltaTime;
			IsAttackReloaded = false;
		}
		else
		{
			IsAttackReloaded = true;
		}
	}
}
