using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ResetGetHitCritter", menuName = "State Machines/Actions/Reset Get Hit of Critter")]
public class ResetGetHitCritterSO : StateActionSO
{
	protected override StateAction CreateAction() => new ResetGetHitCritter();
}

public class ResetGetHitCritter : StateAction
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
