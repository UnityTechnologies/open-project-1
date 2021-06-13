using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsNPCIdle", menuName = "State Machines/Conditions/Is NPC Idle")]
public class IsNPCIdleSO : StateConditionSO<IsNPCIdleCondition>
{
}

public class IsNPCIdleCondition : Condition
{
	//Component references
	private NPC _npcScript;

	public override void Awake(StateMachine stateMachine)
	{
		_npcScript = stateMachine.GetComponent<NPC>();
	}

	protected override bool Statement()
	{

		if (_npcScript.npcState == NPCState.Idle)
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
