using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


public enum ZoneType
{
	Alert,
	Attack
}

[CreateAssetMenu(fileName = "PlayerIsInZone", menuName = "State Machines/Conditions/Player Is In Zone")]
public class PlayerIsInZoneSO : StateConditionSO
{
	public ZoneType zone;

	protected override Condition CreateCondition() => new PlayerIsInZone();
}

public class PlayerIsInZone : Condition
{

	private Critter _critter;

	public override void Awake(StateMachine stateMachine)
	{
		_critter = stateMachine.GetComponent<Critter>();
	}

	protected override bool Statement()
	{
		bool result = false;
		if (_critter != null)
		{
			switch (((PlayerIsInZoneSO)OriginSO).zone)
			{
				case ZoneType.Alert:
					result = _critter.isPlayerInAlertZone;
					break;
				case ZoneType.Attack:
					result = _critter.isPlayerInAttackZone;
					break;
				default:
					break;
			}
		}
		return result;
	}
}
