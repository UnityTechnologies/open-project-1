using System;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "Tree_FellDown", menuName = "State Machines/Actions/Tree_Fell Down")]
public class Tree_FellDownSO : StateActionSO
{
	protected override StateAction CreateAction() => new Tree_FellDown();
}

public class Tree_FellDown : StateAction
{
	private StateMachine stateMachine;

	protected new Tree_FellDownSO OriginSO => (Tree_FellDownSO)base.OriginSO;

	public override void Awake(StateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
	}
	
	public override void OnUpdate()
	{
	}
	
	public override void OnStateEnter()
	{
		GameObject.Destroy(stateMachine.GetComponent<Damageable>().gameObject);
		//TODO animation of falling tree
	}
	
	public override void OnStateExit()
	{
	}
}
