using UnityEngine;
using UnityEngine.Localization;

public class NpcEntity : MonoBehaviour
{
	[Tooltip("The name of the entity")]
	[SerializeField] private LocalizedString _name;

	[Tooltip("The movement strategy and configuration that will define the movment of the entity")]
	[SerializeField]
	private MovementConfigSO _movementConfigSO;

	public MovementConfigSO MovementConfig => _movementConfigSO;
}
