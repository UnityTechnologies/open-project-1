using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Tree_Idle", menuName = "State Machines/Actions/Tree_Idle")]
public class Tree_IdleSO : StateActionSO
{
	protected override StateAction CreateAction() => new Tree_Idle();
}

public class Tree_Idle : StateAction
{
	protected new Tree_IdleSO OriginSO => (Tree_IdleSO)base.OriginSO;

	public override void Awake(StateMachine stateMachine)
	{
	}
	
	public override void OnUpdate()
	{
	}
	
	public override void OnStateEnter()
	{
	}
	
	public override void OnStateExit()
	{
	}
}
