using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChasingConfig", menuName = "Characters/Chasing Config")]
public class ChasingConfigSO : ScriptableObject
{
	[Tooltip("Target transform anchor.")]
	[SerializeField] internal TransformAnchor _targetTransform = default;

	[Tooltip("NPC chasing speed")]
	[SerializeField] internal float _chasingSpeed = default;

	public Vector3 TargetPosition => _targetTransform.Transform.position;
	public float ChasingSpeed => _chasingSpeed;

}

