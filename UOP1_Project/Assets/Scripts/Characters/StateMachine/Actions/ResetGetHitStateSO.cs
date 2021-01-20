using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ResetGetHitState", menuName = "State Machines/Actions/Reset Get Hit State")]
public class ResetGetHitStateSO : StateActionSO
{
	protected override StateAction CreateAction() => new ResetGetHitState();
}

public class ResetGetHitState : StateAction
{
	private Critter _critter;

	public override void Awake(StateMachine stateMachine)
	{
		_critter = stateMachine.GetComponent<Critter>();
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateExit()
	{
		_critter.getHit = false;
	}
}
