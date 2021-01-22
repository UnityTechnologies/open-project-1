using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	private AttackConfigSO _attackConfig;
	private bool _enable = false;
	public bool Enable { get; set; }

	public void Awake()
	{
		_attackConfig = GetComponentInParent<IAttackerCharacter>().getAttackConfig();
	}

	public int AttackStrength
	{
		get
		{
			return _attackConfig.AttackStrength;
		}
	}

}
