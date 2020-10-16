using System;
using DeivSky.StateMachine;
using DeivSky.StateMachine.Scriptables;
using UnityEngine;

[CreateAssetMenu(fileName = "Eat", menuName = "State Machines/Tests/Actions/Eat")]
public class ScriptableEatAction : SerializableStateAction<EatAction> { }

[Serializable]
public class EatAction : StateAction
{
	[SerializeField] private float amount = 10f;
	private HungerComponent _hungerComponent;

	public override void Awake(StateMachine stateMachine)
		=> _hungerComponent = stateMachine.GetComponent<HungerComponent>();

	public override void Perform() { }

	public override void OnStateExit() => _hungerComponent.Eat(amount);
}
