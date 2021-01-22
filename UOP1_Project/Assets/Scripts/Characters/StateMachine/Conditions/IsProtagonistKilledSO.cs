using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsProtagonistKilled", menuName = "State Machines/Conditions/Is Protagonist Killed")]
public class IsProtagonistKilledSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsProtagonistKilled();
}

public class IsProtagonistKilled : Condition
{
	private Protagonist _protagonist;

	public override void Awake(StateMachine stateMachine)
	{
		_protagonist = stateMachine.GetComponent<Protagonist>();
	}

	protected override bool Statement()
	{
		bool result = false;
		if (_protagonist != null)
		{
			result = _protagonist.isDead;
		}
		return result;
	}
}
