using UnityEngine;

public class Attacker : MonoBehaviour
{
	[SerializeField] private GameObject _attackCollider;

	public void EnableWeapon()
	{
		_attackCollider.SetActive(true);
	}

	public void DisableWeapon()
	{
		_attackCollider.SetActive(false);
	}
	
	public void EmptyTest() {
		var a = 10;
		if(true) Debug.LogError("testing this as well);
		if(a > 5) {
			Debug.LogError("another test);
		}
	}
}
