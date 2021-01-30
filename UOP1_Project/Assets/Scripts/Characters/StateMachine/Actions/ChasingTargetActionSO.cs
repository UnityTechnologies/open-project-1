using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ChasingTargetAction", menuName = "State Machines/Actions/Chasing Target Action")]
public class ChasingTargetActionSO : StateActionSO
{
	[Tooltip("Target transform anchor.")]
	[SerializeField] private TransformAnchor _targetTransform = default;

	[Tooltip("NPC chasing speed")]
	[SerializeField] private float _chasingSpeed = default;

	public Vector3 TargetPosition => _targetTransform.Transform.position;
	public float ChasingSpeed => _chasingSpeed;

	protected override StateAction CreateAction() => new ChasingTargetAction();
}

public class ChasingTargetAction : StateAction
{
	private Critter _critter;
	private ChasingTargetActionSO _config;
	private NavMeshAgent _agent;
	private bool _isActiveAgent;

	public override void Awake(StateMachine stateMachine)
	{
		_config = (ChasingTargetActionSO)OriginSO;
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
		_isActiveAgent = _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;
	}

	public override void OnUpdate()
	{
		if (_isActiveAgent)
		{
			_agent.isStopped = false;
			_agent.SetDestination(_config.TargetPosition);
		}
	}

	public override void OnStateEnter()
	{
		if (_isActiveAgent)
		{
			_agent.speed = _config.ChasingSpeed;
		}
	}
}
