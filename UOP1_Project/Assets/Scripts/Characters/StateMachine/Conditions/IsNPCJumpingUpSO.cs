using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is NPC Jumping Up")]
public class IsNPCJumpingUpSO : StateConditionSO<IsNPCJumpingUpCondition> { }

public class IsNPCJumpingUpCondition : Condition
{
	//Component references
	private NPC _npcScript;

	public override void Awake(StateMachine stateMachine)
	{
		_npcScript = stateMachine.GetComponent<NPC>();
	}
	
	protected override bool Statement()
	{

		if (_npcScript.npcState == NPCState.JumpUp)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
}
