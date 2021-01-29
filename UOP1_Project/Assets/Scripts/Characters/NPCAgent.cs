using UnityEngine;
using UnityEngine.Localization;

public class NPCAgent : MonoBehaviour
{
	[Tooltip("The movement strategy and configuration that will define the movment of the entity")]
	[SerializeField]
	private MovementConfigSO _movementConfigSO;

	public MovementConfigSO MovementConfig => _movementConfigSO;
}
