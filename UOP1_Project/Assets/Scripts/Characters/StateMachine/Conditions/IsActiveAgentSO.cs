using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "IsActiveAgent", menuName = "State Machines/Conditions/Is Active Agent")]
public class IsActiveAgentSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsActiveAgent();
}

public class IsActiveAgent : Condition
{
	private NavMeshAgent _agent;

	public override void Awake(StateMachine stateMachine)
	{
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
	}

	protected override bool Statement()
	{
		return _agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh;
	}
}
