using UnityEngine;
using UnityEngine.Localization;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HasTheLineBeenSaid", menuName = "State Machines/Conditions/Has The Line Been Said")]

public class HasTheLineBeenSaidSO : StateConditionSO<HasTheLineBeenSaidCondition> { }

public class HasTheLineBeenSaidCondition : Condition
{
	//Component references
	private NPC _npcScript;

	public override void Awake(StateMachine stateMachine)
	{
		_npcScript = stateMachine.GetComponent<NPC>();
	}

	protected override bool Statement()
	{

		if (_npcScript.hasSaidLine)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
