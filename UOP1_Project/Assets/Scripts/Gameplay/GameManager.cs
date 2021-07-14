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
	private VoidEventChannelSO _addRockCandyRecipeEvent = default;

	[SerializeField]
	private ItemSO _rockCandyRecipe = default;

	[SerializeField]
	private InventorySO _inventory = default;

	private void Start()
	{
		StartGame();

	}
	private void OnEnable()
	{
		_addRockCandyRecipeEvent.OnEventRaised += AddRockCandyRecipe; 
	}
	private void OnDisable()
	{
		_addRockCandyRecipeEvent.OnEventRaised -= AddRockCandyRecipe;

	}
	void AddRockCandyRecipe()
	{
		_inventory.Add(_rockCandyRecipe);

	}
	// Start is called before the first frame update
	void StartGame()
    {
        _gameState.UpdateGameState(GameState.Gameplay); 
        _questManager.StartGame(); 
    }
    public void PauseGame()
	{
        	}
    public void UnpauseGame()
    {
        _gameState.ResetToPreviousGameState(); 
    }

}
