using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ResetHealth", menuName = "State Machines/Actions/Reset Health")]
public class ResetHealthSO : StateActionSO
{
	protected override StateAction CreateAction() => new ResetHealth();
}

public class ResetHealth : StateAction
{
	private Damageable _damageableEntity;

	public override void Awake(StateMachine stateMachine)
	{
		_damageableEntity = stateMachine.GetComponent<Damageable>();
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateExit()
	{
		_damageableEntity.Revive();
	}
}
