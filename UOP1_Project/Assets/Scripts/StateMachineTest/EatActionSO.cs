using DeivSky.StateMachine;
using DeivSky.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Eat", menuName = "State Machines/Tests/Actions/Eat")]
public class EatActionSO : StateActionSO
{
	public float amount = 10f;

	protected override StateAction CreateAction() => new EatAction(amount);
}

public class EatAction : StateAction
{
	private float _amount = 10f;
	private HungerComponent _hungerComponent;

	public EatAction(float amount) => _amount = amount;

	public override void Awake(StateMachine stateMachine)
		=> _hungerComponent = stateMachine.GetComponent<HungerComponent>();

	public override void OnUpdate() { }

	public override void OnStateExit() => _hungerComponent.Eat(_amount);
}
