using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Runtime Anchors/Health")]
public class HealthAnchor : RuntimeAnchorBase
{
	[SerializeField]
	private int _MaxHealth;

	private int _currenHealth;

	public int CurrentHealth { get => _currenHealth; }

	public void FillHealth()
	{
		_currenHealth = _MaxHealth;
	}

	public void DecreaseHealth(int damage)
	{
		_currenHealth -= damage;
	}

	public bool isDead()
	{
		return _currenHealth <= 0;
	}
}
