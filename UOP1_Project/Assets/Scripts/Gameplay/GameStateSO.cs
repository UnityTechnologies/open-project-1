using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
	Gameplay,// regular state: player moves, attacks, can perform actions
	Pause,// pause menu is opened, the whole game world is frozen
	Inventory, //when inventory UI or cooking UI are open
	Dialogue,
	Cutscene,
	LocationTransition,// when the character steps into LocationExit trigger, fade to black begins and control is removed from the player
	Combat,//enemy is nearby and alert, player can't open Inventory or initiate dialogues, but can pause the game
}

//[CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/GameState", order = 51)]
public class GameStateSO : ScriptableObject
{
	private GameState _currentGameState = default;
	private GameState _previousGameState = default;

	[SerializeField] BoolEventChannelSO _setAggroEvent = default;
	public GameState CurrentGameState => _currentGameState;
	List<Transform> _enemiesInCombat = new List<Transform>();
	private void OnEnable()
	{
		_enemiesInCombat = new List<Transform>();

	}
	public void ChangeStateToCombat(Transform enemy)
	{
		if (!_enemiesInCombat.Exists(o => o == enemy))
		{
			_enemiesInCombat.Add(enemy);
		}

		UpdateGameState(GameState.Combat);
	}
	public void ChangeStateFromCombat(Transform enemy)
	{

		if (_enemiesInCombat.Exists(o => o == enemy))
		{
			_enemiesInCombat.Remove(enemy);


			if (_enemiesInCombat.Count > 0)
			{
				UpdateGameState(GameState.Combat);
			}
			else
			{
				UpdateGameState(GameState.Gameplay);
			}
		}
	}
	public void UpdateGameState(GameState newGameState)
	{

		if (newGameState != CurrentGameState)
		{
			if (newGameState == GameState.Combat) // if new game state is combat : switch to aggro
			{
				_setAggroEvent.RaiseEvent(true);
			}
			else
			{
				_setAggroEvent.RaiseEvent(false);
			}

			_previousGameState = _currentGameState;
			_currentGameState = newGameState;
		}
	}
	public void ResetToPreviousGameState()
	{
		if (_previousGameState != CurrentGameState)
		{
			if (_previousGameState == GameState.Combat) // if new game state is combat : switch to aggro
			{
				_setAggroEvent.RaiseEvent(true);
			}
			else if (CurrentGameState == GameState.Combat) // if current game state is combat then switch out of aggro 
			{
				_setAggroEvent.RaiseEvent(false);
			}
			_currentGameState = _previousGameState;
		}
	}

}
