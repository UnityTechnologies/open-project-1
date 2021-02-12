using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "IsAttackReloadedCondition", menuName = "State Machines/Conditions/Is Attack Reloaded")]
public class IsAttackReloadedConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsAttackReloadedCondition();
}

public class IsAttackReloadedCondition : Condition
{
	private Attacker _attacker;

	public override void Awake(StateMachine stateMachine)
	{
		_attacker = stateMachine.GetComponent<Attacker>();
	}

	protected override bool Statement()
	{
		return _attacker.IsAttackReloaded;
	}
}
