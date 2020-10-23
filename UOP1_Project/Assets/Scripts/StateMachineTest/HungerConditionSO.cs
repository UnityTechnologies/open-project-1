using DeivSky.StateMachine;
using DeivSky.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "IsHungry", menuName = "State Machines/Tests/Conditions/IsHungry")]
public class HungerConditionSO : StateConditionSO<HungerCondition> { }

public class HungerCondition : Condition
{
	private HungerComponent _hungerComponent;

	public override void Awake(StateMachine stateMachine)
	{ 
		_hungerComponent = stateMachine.GetComponent<HungerComponent>();
	}

	public override bool Statement() => _hungerComponent.IsHungry;
}
