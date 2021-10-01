using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private QuestManagerSO _questManager = default;
	[SerializeField] private GameStateSO _gameState = default;

	[Header("Inventory")]
	[SerializeField] private ItemSO _rockCandyRecipe = default;
	[SerializeField] private ItemSO _sweetDoughRecipe = default;
	[SerializeField] private ItemSO[] _finalRecipes = default;
	[SerializeField] private InventorySO _inventory = default;

	[Header("Broadcasting on")]
	[SerializeField] private VoidEventChannelSO _addRockCandyRecipeEvent = default;
	[SerializeField] private VoidEventChannelSO _cerisesMemoryEvent = default;
	[SerializeField] private VoidEventChannelSO _decideOnDishesEvent = default;

	private void Start()
	{
		StartGame();
	}

	private void OnEnable()
	{
		_addRockCandyRecipeEvent.OnEventRaised += AddRockCandyRecipe;
		_cerisesMemoryEvent.OnEventRaised += AddSweetDoughRecipe;
		_decideOnDishesEvent.OnEventRaised += AddFinalRecipes;
	}

	private void OnDisable()
	{
		_addRockCandyRecipeEvent.OnEventRaised -= AddRockCandyRecipe;
		_cerisesMemoryEvent.OnEventRaised -= AddSweetDoughRecipe;
		_decideOnDishesEvent.OnEventRaised -= AddFinalRecipes;
	}

	void AddRockCandyRecipe()
	{
		_inventory.Add(_rockCandyRecipe);
	}

	void AddSweetDoughRecipe()
	{
		_inventory.Add(_sweetDoughRecipe);
	}

	void AddFinalRecipes()
	{
		foreach (ItemSO item in _finalRecipes)
		{
			_inventory.Add(item);
		}
	}

	void StartGame()
	{
		_gameState.UpdateGameState(GameState.Gameplay);
		_questManager.StartGame();
	}
}
