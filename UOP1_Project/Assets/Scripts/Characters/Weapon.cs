using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] private int _attackStrength = default;

	private bool _enable = false;
	public bool Enable
	{
		get => _enable;
		set => _enable = value;
	}	

	public int AttackStrength
	{
		get => _attackStrength;
	}

}
