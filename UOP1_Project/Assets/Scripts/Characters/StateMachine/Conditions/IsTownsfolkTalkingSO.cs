using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Townsfolk Talking")]
public class IsTalkingSO : StateConditionSO<IsTownsfolkTalkingCondition> { }

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

		if (_townsfolkScript.isTalking)
		{
			// Consume it
			_townsfolkScript.isTalking = false;

			return true;
		}
		else
		{
			return false;
		}
	}
	
}
