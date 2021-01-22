using UnityEngine;
using System.Collections;

public interface IAttackerCharacter
{
	AttackConfigSO getAttackConfig();

	bool IsReadyToAttack();

	void TriggerAttack();

}
