using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ActivateAttack", menuName = "State Machines/Actions/Activate Attack")]
public class ActivateAttackSO : StateActionSO
{
	protected override StateAction CreateAction() => new ActivateAttack();
}

public class ActivateAttack : StateAction
{
	private Attack _attack;

	public override void Awake(StateMachine stateMachine)
	{
		_attack = stateMachine.gameObject.GetComponentInChildren<Attack>();
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		if (_attack != null)
		{
			_attack.Enable = true;
		}
	}

	public override void OnStateExit()
	{
		if (_attack != null)
		{
			_attack.Enable = false;
		}
	}
}
