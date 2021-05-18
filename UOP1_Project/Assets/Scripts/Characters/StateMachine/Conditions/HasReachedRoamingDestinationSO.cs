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
	private bool _agentDefined;

	public override void Awake(StateMachine stateMachine)
	{
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
		_agentDefined = _agent != null;
	}

	protected override bool Statement()
	{
		//Debug.Log("This agent is defined" + _agentDefined);
		//Debug.Log("This agent has path " + _agent.hasPath);
		Debug.Log("distance remaining" + _agent.remainingDistance);
		return _agent.remainingDistance < 0.01;
		//value to use 0.1?  and has path 
		//!_agent.hasPath ||
	}
}
