using System;
using DeivSky.StateMachine;
using DeivSky.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase", menuName = "State Machines/Tests/Actions/Chase")]
public class ChaseActionSO : StateActionSO<ChaseAction> { }

public class ChaseAction : StateAction
{
	private ChaseComponent _chaseComponent;

	public override void Awake(StateMachine stateMachine)
	{
		_chaseComponent = stateMachine.GetComponent<ChaseComponent>();
	}

	public override void OnUpdate()
	{
		_chaseComponent.Chase();
	}
}
