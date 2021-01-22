using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChasingConfig", menuName = "Characters/Chasing Config")]
public class ChasingConfigSO : ScriptableObject
{
	[Tooltip("Target transform anchor.")]
	[SerializeField] internal TransformAnchor _targetTransform = default;

	[Tooltip("NPC chasing speed")]
	[SerializeField] internal float _chasingSpeed = default;

	public void ChasingTarget(NavMeshAgent agent)
	{
		agent.speed = _chasingSpeed;
		agent.SetDestination(_targetTransform.Transform.position);
	}

}

