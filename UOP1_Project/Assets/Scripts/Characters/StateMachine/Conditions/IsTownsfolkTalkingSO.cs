using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Townsfolk Talking")]
public class IsTownsfolkTalkingSO : StateConditionSO<IsTownsfolkTalkingCondition> { }

public class IsTownsfolkTalkingCondition : Condition
{
	//Component references
	private Townsfolk _townsfolkScript;

	public override void Awake(StateMachine stateMachine)
	{
		_townsfolkScript = stateMachine.GetComponent<Townsfolk>();
	}
	
	protected override bool Statement()
	{

		if (_townsfolkScript.townsfolkInitialState == InitialState.Talk)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
}
