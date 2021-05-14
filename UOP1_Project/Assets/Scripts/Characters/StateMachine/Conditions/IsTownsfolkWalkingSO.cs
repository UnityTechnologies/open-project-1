using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Townsfolk Walking")]
public class IsTownsfolkWalkingSO : StateConditionSO<IsTownsfolkWalkingCondition> { }

public class IsTownsfolkWalkingCondition : Condition
{
	//Component references
	private Townsfolk _townsfolkScript;

	public override void Awake(StateMachine stateMachine)
	{
		_townsfolkScript = stateMachine.GetComponent<Townsfolk>();
	}
	
	protected override bool Statement()
	{

		if (_townsfolkScript.townsfolkState == TownsfolkState.Walk)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
}
