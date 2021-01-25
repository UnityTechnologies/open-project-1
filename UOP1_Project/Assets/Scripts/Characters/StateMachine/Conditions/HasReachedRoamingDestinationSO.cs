using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "HasReachedRoamingDestination", menuName = "State Machines/Conditions/Has Reached Roaming Destination")]
public class HasReachedRoamingDestinationSO : StateConditionSO
{
	protected override Condition CreateCondition() => new HasReachedRoamingDestination();
}

public class HasReachedRoamingDestination : Condition
{
	private NavMeshAgent _agent;
	private float _startTime;

	public override void Awake(StateMachine stateMachine)
	{
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
	}

	protected override bool Statement()
	{
		return _agent == null || !_agent.hasPath;
	}
}
