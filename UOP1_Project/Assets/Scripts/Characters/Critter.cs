using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : MonoBehaviour
{
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

}
