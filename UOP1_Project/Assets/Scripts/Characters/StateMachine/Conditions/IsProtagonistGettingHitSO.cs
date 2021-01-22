using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "IsProtagonistGettingHit", menuName = "State Machines/Conditions/Is Protagonist Getting Hit")]
public class IsProtagonistGettingHitSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsProtagonistGettingHit();
}

public class IsProtagonistGettingHit : Condition
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
			result = _protagonist.getHit;
		}
		return result;
	}
}
