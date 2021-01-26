using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "RoamingAroundSpawningPositionAction", menuName = "State Machines/Actions/Roaming Around Spawning Position Action")]
public class RoamingAroundSpawningPositionActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new RoamingAroundSpawningPositionAction();
}

public class RoamingAroundSpawningPositionAction : StateAction
{
	private NavMeshAgent _agent;
	private bool _isActiveAgent;
	private Vector3 _startPosition;

	private float _roamingSpeed;
	private float _roamingDistance;

	private Vector3 _roamingTargetPosition;

	public override void Awake(StateMachine stateMachine)
	{
		RoamingAroundSpawningPositionConfigSO config = stateMachine.GetComponent<Critter>().RoamingAroundSpawningPositionConfig;
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;
		_startPosition = stateMachine.transform.position;
		_roamingSpeed = config.RoamingSpeed;
		_roamingDistance = config.RoamingDistance;
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		if (_isActiveAgent)
		{
			_roamingTargetPosition = GetRoamingPositionAroundPosition(_startPosition);
			_agent.speed = _roamingSpeed;
			_agent.isStopped = false;
			_agent.SetDestination(_roamingTargetPosition);
		}
	}

	// Compute a random target position around the starting position.
	private Vector3 GetRoamingPositionAroundPosition(Vector3 position)
	{
		return position + new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized * Random.Range(_roamingDistance / 2, _roamingDistance);
	}
}
