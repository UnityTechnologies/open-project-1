using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "EnableHunger", menuName = "State Machines/Tests/Actions/Enable Hunger")]
public class HungerActionSO : StateActionSO<HungerAction> { }

public class HungerAction : StateAction
{
	private HungerComponent _hungerComponent;

	public override void Awake(StateMachine stateMachine)
		=> _hungerComponent = stateMachine.GetComponent<HungerComponent>();

	public override void OnUpdate() { }

	public override void OnStateEnter() => _hungerComponent.ToggleHunger(true);
	public override void OnStateExit() => _hungerComponent.ToggleHunger(false);
}
