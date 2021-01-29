using UnityEngine;
using UnityEngine.AI;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "NpcMovementStrategyAction", menuName = "State Machines/Actions/Appy NPC Movement Action")]
public class NpcMovementStrategyActionSO : StateActionSO
{
	protected override StateAction CreateAction() => new MovementStrategyAction();
}

public class MovementStrategyAction : StateAction
{
	IMovementActionStrategy _movementStrategy;

	public override void Awake(StateMachine stateMachine)
	{
		MovementConfigSO config = stateMachine.GetComponent<NPCAgent>().MovementConfig;
		if (typeof(StaticMovementConfigSO).IsInstanceOfType(config))
		{
			_movementStrategy = new StaticMovementActionStrategy(stateMachine.gameObject, (StaticMovementConfigSO)config);
		}
		else if (typeof(RoamingAroundSpawningPositionConfigSO).IsInstanceOfType(config))
		{
			_movementStrategy = new RoamingAroundSpawningPositionActionStrategy(stateMachine.gameObject, (RoamingAroundSpawningPositionConfigSO)config);
		}
	}

	public override void OnUpdate()
	{
		_movementStrategy.ApplyMovementOnUpdate();
	}

	public override void OnStateEnter()
	{
		_movementStrategy.ApplyMovementOnStateEnter();
	}

	public override void OnStateExit()
	{
		_movementStrategy.ApplyMovementOnStateExit();
	}
}
