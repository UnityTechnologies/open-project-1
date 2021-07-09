using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsPressedCondition", menuName = "State Machines/Conditions/Is Pressed")]
public class IsPressedConditionSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsPressedCondition();
}

public class IsPressedCondition : Condition
{
	private PressurePlate _pressurePlateScript;

	public override void Awake(StateMachine stateMachine)
	{
		_pressurePlateScript = stateMachine.GetComponent<PressurePlate>();
	}

	protected override bool Statement()
	{
		return _pressurePlateScript.IsPressed;
	}
}
