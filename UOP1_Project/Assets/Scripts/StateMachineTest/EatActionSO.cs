using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Eat", menuName = "State Machines/Tests/Actions/Eat")]
public class EatActionSO : StateActionSO<EatAction> { }

public class EatAction : StateAction
{
	private HungerComponent _hungerComponent;

	public override void Awake(StateMachine stateMachine)
	{
		_hungerComponent = stateMachine.GetComponent<HungerComponent>();
	}

	public override void OnUpdate() { }

	public override void OnStateExit()
	{
		_hungerComponent.Eat();
	}
}
