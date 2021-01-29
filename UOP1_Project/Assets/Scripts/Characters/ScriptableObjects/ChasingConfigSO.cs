using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChasingConfig", menuName = "EntityConfig/Chasing Config")]
public class ChasingConfigSO : ScriptableObject
{
	[Tooltip("Target transform anchor.")]
	[SerializeField] private TransformAnchor _targetTransform = default;

	[Tooltip("NPC chasing speed")]
	[SerializeField] private float _chasingSpeed = default;

	public Vector3 TargetPosition => _targetTransform.Transform.position;
	public float ChasingSpeed => _chasingSpeed;

}

