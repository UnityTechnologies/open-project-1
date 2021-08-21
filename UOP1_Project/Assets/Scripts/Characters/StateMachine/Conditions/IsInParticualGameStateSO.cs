using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is In Particular GameState")]
public class IsInParticualGameStateSO : StateConditionSO<IsInParticualGameStateCondition>
{
	public GameState _currentGameState;
	public GameStateSO _gameStateManager;

	protected override Condition CreateCondition() => new IsInParticualGameStateCondition();
}

public class IsInParticualGameStateCondition : Condition
{
	private IsInParticualGameStateSO _originSO => (IsInParticualGameStateSO)base.OriginSO; // The SO this Condition spawned from

	public override void Awake(StateMachine stateMachine)
	{

	}

	protected override bool Statement()
	{
		return _originSO._currentGameState == _originSO._gameStateManager.CurrentGameState;
	}

	private void EventReceived()
	{

	}

	public override void OnStateExit()
	{
	}
}
