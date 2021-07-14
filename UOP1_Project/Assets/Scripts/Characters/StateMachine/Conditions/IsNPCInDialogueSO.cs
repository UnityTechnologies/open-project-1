using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is NPC In Dialogue")]
public class IsNPCInDialogueSO : StateConditionSO<IsNPCDialogueCondition> { }

public class IsNPCDialogueCondition : Condition
{
	//Component references
	private StepController _stepControllerScript;

	public override void Awake(StateMachine stateMachine)
	{
		_stepControllerScript = stateMachine.GetComponent<StepController>();
	}
	
	protected override bool Statement()
	{

		if (_stepControllerScript.isInDialogue)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
}
