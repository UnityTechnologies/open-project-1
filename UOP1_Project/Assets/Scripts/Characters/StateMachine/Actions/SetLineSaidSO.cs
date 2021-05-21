using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "SetLineSaidToTrue", menuName = "State Machines/Actions/Set Line Said To True SO")]
public class SetLineSaidSO : StateActionSO
{
	protected override StateAction CreateAction() => new SetLineSaid();
}

public class SetLineSaid : StateAction
{
	private NPC _npcScript;


	public override void Awake(StateMachine stateMachine)
	{
		_npcScript = stateMachine.gameObject.GetComponent<NPC>();
	}

	public override void OnUpdate()
	{

	}


	public override void OnStateExit()
	{
		_npcScript.hasSaidLine = true;
	}
}
