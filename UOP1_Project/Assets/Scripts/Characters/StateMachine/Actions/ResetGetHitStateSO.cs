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
	private AttackableEntity _attackableEntity;

	public override void Awake(StateMachine stateMachine)
	{
		_attackableEntity = stateMachine.GetComponent<AttackableEntity>();
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateExit()
	{
		_attackableEntity.getHit = false;
	}
}
