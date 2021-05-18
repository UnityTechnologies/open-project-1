using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private QuestManagerSO _questManager = default;

    [SerializeField]
    private GameStateSO _gameState = default;
    [SerializeField]
    private InputReader _inputReader = default; 

    private void Start()
	{
        StartGame();

        _inputReader.pauseEvent += PauseGame; 

    }
	// Start is called before the first frame update
	void StartGame()
    {
        _gameState.UpdateGameState(GameState.Gameplay); 
        _questManager.StartGame(); 
    }
    public void PauseGame()
	{
        _gameState.UpdateGameState(GameState.Pause); 
	}
    public void UnauseGame()
    {
        _gameState.ResetToPreviousGameState(); 
    }

}
