using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is In Particular GameState")]
public class IsInSpecificGameStateSO : StateConditionSO<IsInSpecificGameStateCondition>
{
	[UnityEngine.Serialization.FormerlySerializedAs("_currentGameState")] public GameState gameStateToCheck;
	[UnityEngine.Serialization.FormerlySerializedAs("gameStateManager")] public GameStateSO gameStateSO;

	protected override Condition CreateCondition() => new IsInSpecificGameStateCondition();
}

public class IsInSpecificGameStateCondition : Condition
{
	private IsInSpecificGameStateSO _originSO => (IsInSpecificGameStateSO)base.OriginSO; // The SO this Condition spawned from

	protected override bool Statement()
	{
		return _originSO.gameStateToCheck == _originSO.gameStateSO.CurrentGameState;
	}
}
