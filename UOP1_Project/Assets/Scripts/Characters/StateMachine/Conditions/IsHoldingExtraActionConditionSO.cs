/* Unused. Kept for reference

using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Holding ExtraAction")]
public class IsHoldingExtraActionConditionSO : StateConditionSO<IsHoldingExtraActionCondition> { }

public class IsHoldingExtraActionCondition : Condition
{
	//Component references
	private Protagonist _protagonistScript;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	protected override bool Statement()
	{
		if (_protagonistScript.extraActionInput)
		{
			// Consume the input
			_protagonistScript.extraActionInput = false;

			return true;
		}
		else
		{
			return false;
		}
	}
}

*/
