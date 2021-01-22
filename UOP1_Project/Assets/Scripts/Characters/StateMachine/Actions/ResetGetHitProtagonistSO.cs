using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ResetGetHitProtagonist", menuName = "State Machines/Actions/Reset Get Hit of Protagonist")]
public class ResetGetHitProtagonistSO : StateActionSO
{
	protected override StateAction CreateAction() => new ResetGetHitProtagonist();
}

public class ResetGetHitProtagonist : StateAction
{
	private Protagonist _protagonist;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonist = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{

	}

	public override void OnStateExit()
	{
		_protagonist.getHit = false;
	}
}
