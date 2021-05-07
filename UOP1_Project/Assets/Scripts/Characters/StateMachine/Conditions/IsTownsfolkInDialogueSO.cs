using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Townsfolk In Dialogue")]
public class IsTownsfolkInDialogueSO : StateConditionSO<IsTownsfolkDialogueCondition> { }

public class IsTownsfolkDialogueCondition : Condition
{
	//Component references
	private StepController _stepControllerScript;

	public override void Awake(StateMachine stateMachine)
	{
		_stepControllerScript = stateMachine.GetComponent<StepController>();
	}
	
	protected override bool Statement()
	{

		if (_stepControllerScript.IsInDialogue)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
}
