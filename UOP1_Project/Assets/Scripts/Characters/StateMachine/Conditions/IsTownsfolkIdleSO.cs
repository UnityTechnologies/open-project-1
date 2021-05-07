using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsTownsfolkIdle", menuName = "State Machines/Conditions/Is Townsfolk Idle")]
public class IsTownsfolkIdleSO : StateConditionSO<IsTownsfolkIdleCondition>
{
}

public class IsTownsfolkIdleCondition : Condition
{
	//Component references
	private Townsfolk _townsfolkScript;

	public override void Awake(StateMachine stateMachine)
	{
		_townsfolkScript = stateMachine.GetComponent<Townsfolk>();
	}

	protected override bool Statement()
	{

		if (_townsfolkScript.isIdle)
		{
			// We don't want to consume it because we want the townsfolk to stay idle
			return true;
		}
		else
		{
			return false;
		}
	}
}
