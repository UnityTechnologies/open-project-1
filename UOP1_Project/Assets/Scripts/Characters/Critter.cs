using UnityEngine;

public class Critter : MonoBehaviour
{
	[SerializeField]
	private ChasingConfigSO _chasingConfigSO;

	public bool isPlayerInAlertZone { get; set; }
	public bool isPlayerInAttackZone { get; set; }

	public ChasingConfigSO ChasingConfig => _chasingConfigSO;
}
