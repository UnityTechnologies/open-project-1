using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	[SerializeField]
	private AttackConfigSO _attackConfigSO;

	private bool _enable = false;
	public bool Enable { get; set; }

	public AttackConfigSO AttackConfig => _attackConfigSO;

}
