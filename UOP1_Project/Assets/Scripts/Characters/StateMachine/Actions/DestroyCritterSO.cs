using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "DestroyCritter", menuName = "State Machines/Actions/Destroy Critter")]
public class DestroyCritterSO : StateActionSO
{
	protected override StateAction CreateAction() => new DestroyCritter();
}

public class DestroyCritter : StateAction
{
	private Critter _critter;

	public override void Awake(StateMachine stateMachine)
	{
		_critter = stateMachine.GetComponent<Critter>();
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		_critter.DestroyCritter();
	}
}
