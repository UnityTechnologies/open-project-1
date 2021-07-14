using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "HasReachedRoamingDestination", menuName = "State Machines/Conditions/Has Reached Waypoint")]
public class HasReachedWaypointSO : StateConditionSO
{
	protected override Condition CreateCondition() => new HasReachedWaypoint();
}

public class HasReachedWaypoint : Condition
{
	private NavMeshAgent _agent;

	public override void Awake(StateMachine stateMachine)
	{
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
	}

	protected override bool Statement()
	{
		if (!_agent.pathPending)
		{
			//set the stop distance to 0.1 if it is set to 0 in the inspector 
			if (_agent.stoppingDistance == 0) _agent.stoppingDistance = 0.1f;
			if (_agent.remainingDistance <= _agent.stoppingDistance)
			{
				if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
				{
					return true;
				}
			}
		}
		return false;
	}
}
