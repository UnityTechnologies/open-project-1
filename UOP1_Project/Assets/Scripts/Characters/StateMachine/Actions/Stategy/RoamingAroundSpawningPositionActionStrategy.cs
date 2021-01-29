using UnityEngine;
using UnityEngine.AI;

public class RoamingAroundSpawningPositionActionStrategy : IMovementActionStrategy
{
	private NavMeshAgent _agent;
	private bool _isActiveAgent;
	private Vector3 _startPosition;

	private float _roamingSpeed;
	private float _roamingDistance;

	private Vector3 _roamingTargetPosition;

	public RoamingAroundSpawningPositionActionStrategy(GameObject gameObject, RoamingAroundSpawningPositionConfigSO config)
	{
		_agent = gameObject.GetComponent<NavMeshAgent>();
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;
		_startPosition = gameObject.transform.position;
		_roamingSpeed = config.RoamingSpeed;
		_roamingDistance = config.RoamingDistance;
	}

	public void ApplyMovementOnStateEnter()
	{
		if (_isActiveAgent)
		{
			_roamingTargetPosition = GetRoamingPositionAroundPosition(_startPosition);
			_agent.speed = _roamingSpeed;
			_agent.isStopped = false;
			_agent.SetDestination(_roamingTargetPosition);
		}
	}

	public void ApplyMovementOnStateExit()
	{

	}

	public void ApplyMovementOnUpdate()
	{

	}

	// Compute a random target position around the starting position.
	private Vector3 GetRoamingPositionAroundPosition(Vector3 position)
	{
		return position + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized * Random.Range(_roamingDistance / 2, _roamingDistance);
	}
}
