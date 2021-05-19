using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "IsPathReady", menuName = "State Machines/Conditions/Is Path Ready")]
public class IsPathReadySO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsPathReady();
}

public class IsPathReady : Condition
{
	private NavMeshAgent _agent;

	public override void Awake(StateMachine stateMachine)
	{
		_agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
	}

	protected override bool Statement()
	{
		return _agent.pathPending;
	}
}
