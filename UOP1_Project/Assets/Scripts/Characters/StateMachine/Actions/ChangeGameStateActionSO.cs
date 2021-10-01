using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;
using Moment = UOP1.StateMachine.StateAction.SpecificMoment;

/// <summary>
/// This Action handles updating the game state.
/// </summary>
[CreateAssetMenu(fileName = "ChangeGameState", menuName = "State Machines/Actions/Change GameState")]
public class ChangeGameStateActionSO : StateActionSO
{
	[SerializeField] GameState _newGameState = default;
	[SerializeField] Moment _whenToRun = default;
	[SerializeField] private GameStateSO _gameState = default;
	
	protected override StateAction CreateAction() => new ChangeGameStateAction(_newGameState, _gameState, _whenToRun);
}

public class ChangeGameStateAction : StateAction
{
	[Tooltip("GameState to change to")]
	private GameState _newGameState = default;
	private GameStateSO _gameStateSO = default;
	private Moment _whenToRun = default;
	private Transform _transform = default;

	public ChangeGameStateAction(GameState newGameState, GameStateSO gameStateSO, Moment whenToRun)
	{
		_newGameState = newGameState;
		_gameStateSO = gameStateSO;
		_whenToRun = whenToRun;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_transform = stateMachine.transform;
	}

	void ChangeState()
	{
		switch (_newGameState)
		{
			case GameState.Combat:
				_gameStateSO.AddAlertEnemy(_transform);
				break;

			case GameState.Gameplay:
				_gameStateSO.RemoveAlertEnemy(_transform);
				break;

			default:
				_gameStateSO.UpdateGameState(_newGameState);
				break;
		}
	}

	public override void OnStateEnter()
	{
		if (_whenToRun == Moment.OnStateEnter)
		{
			ChangeState();
		}
	}

	public override void OnStateExit()
	{
		if (_whenToRun == Moment.OnStateExit)
		{
			ChangeState();
		}
	}
	
	public override void OnUpdate(){ }
}
