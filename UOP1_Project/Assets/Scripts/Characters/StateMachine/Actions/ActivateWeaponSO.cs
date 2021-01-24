using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ActivateWeapon", menuName = "State Machines/Actions/Activate Weapon")]
public class ActivateWeaponSO : StateActionSO
{
	protected override StateAction CreateAction() => new ActivateWeapon();
}

public class ActivateWeapon : StateAction
{
	private Weapon _weapon;

	public override void Awake(StateMachine stateMachine)
	{
		_weapon = stateMachine.gameObject.GetComponentInChildren<Weapon>();
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		if (_weapon != null)
		{
			_weapon.Enable = true;
		}
	}

	public override void OnStateExit()
	{
		if (_weapon != null)
		{
			_weapon.Enable = false;
		}
	}
}
