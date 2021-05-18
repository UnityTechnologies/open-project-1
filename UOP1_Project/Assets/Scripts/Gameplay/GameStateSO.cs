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
    public GameState CurrentGameState => CurrentGameState;

    public void UpdateGameState( GameState newGameState)
	{
           _previousGameState = _currentGameState; 
           _currentGameState = newGameState; 

    }
    public void ResetToPreviousGameState()
    {
        _currentGameState = _previousGameState;

    }

}
