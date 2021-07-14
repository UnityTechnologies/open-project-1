using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is NPC Walking")]
public class IsNPCWalkingSO : StateConditionSO<IsNPCWalkingCondition> { }

public class IsNPCWalkingCondition : Condition
{
	//Component references
	private NPC _npcScript;

	public override void Awake(StateMachine stateMachine)
	{
		_npcScript = stateMachine.GetComponent<NPC>();
	}
	
	protected override bool Statement()
	{

		if (_npcScript.npcState == NPCState.Walk)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
}
