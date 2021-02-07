using UnityEngine;

public class Attacker : MonoBehaviour
{
	[SerializeField]
	private Attack _attack;

	public void EnableWeapon()
	{
		_attack.Enable = true;
	}

	public void DisableWeapon()
	{
		_attack.Enable = false;
	}
}
