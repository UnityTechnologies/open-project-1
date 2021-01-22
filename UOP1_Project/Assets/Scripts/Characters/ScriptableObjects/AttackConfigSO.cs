using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "Characters/Attack Config")]
public class AttackConfigSO : ScriptableObject
{
	[Tooltip("Character attack strength")]
	[SerializeField] private int _attackStrength;

	[Tooltip("Character attack reload duration (in second).")]
	[SerializeField] private float _attackReloadDuration;

	public int AttackStrength => _attackStrength;

	public AttackData createAttackData()
	{
		return new AttackData();
	}

	public void computeAttackCapability(AttackData data)
	{
		if (data.innerReloadingTime > 0)
		{
			data.innerReloadingTime -= Time.deltaTime;
			data.readyToAttack = false;
		}
		else
		{
			data.readyToAttack = true;
		}
	}

	public void Attack(AttackData data)
	{
		data.innerReloadingTime = _attackReloadDuration;
		data.readyToAttack = false;
	}

	public bool isReadyToAttack(AttackData data)
	{
		return data.readyToAttack;
	}
}

public class AttackData
{
	internal bool readyToAttack = true;
	internal float innerReloadingTime = 0.0f;
}
