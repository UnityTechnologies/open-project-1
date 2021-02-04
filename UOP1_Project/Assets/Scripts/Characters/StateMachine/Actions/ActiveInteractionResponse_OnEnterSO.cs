using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ActiveInteractionResponse_OnEnter", menuName = "State Machines/Actions/Active Interaction Response On Enter")]
public class ActiveInteractionResponse_OnEnterSO : StateActionSO
{
	public VoidEventChannelSO interactResponse;
	protected override StateAction CreateAction() => new ActiveInteractionResponse_OnEnter();
}

public class ActiveInteractionResponse_OnEnter : StateAction
{
	protected new ActiveInteractionResponse_OnEnterSO OriginSO => (ActiveInteractionResponse_OnEnterSO)base.OriginSO;
	
	public override void Awake(StateMachine stateMachine)
	{
	}
	
	public override void OnUpdate()
	{
	}
	
	public override void OnStateEnter()
	{
		OriginSO.interactResponse.RaiseEvent();
	}
	
	public override void OnStateExit()
	{
	}
}
