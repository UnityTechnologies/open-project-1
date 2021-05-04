using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scene loading and unloading.
/// </summary>
public class SceneLoader : MonoBehaviour
{
	[SerializeField] private GameSceneSO _gameplayScene = default;

	[Header("Load Events")]
	[SerializeField] private LoadEventChannelSO _loadLocation = default;
	[SerializeField] private LoadEventChannelSO _loadMenu = default;
	[SerializeField] private LoadEventChannelSO _coldStartupLocation = default;

	[Header("Broadcasting on")]
	[SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

	private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
	private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

	//Parameters coming from scene loading requests
	private GameSceneSO _sceneToLoad;
	private GameSceneSO _currentlyLoadedScene;
	private bool _showLoadingScreen;

	private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();

	private void OnEnable()
	{
		_loadLocation.OnLoadingRequested += LoadLocation;
		_loadMenu.OnLoadingRequested += LoadMenu;
#if UNITY_EDITOR
		_coldStartupLocation.OnLoadingRequested += LocationColdStartup;
#endif
	}

	private void OnDisable()
	{
		_loadLocation.OnLoadingRequested -= LoadLocation;
		_loadMenu.OnLoadingRequested -= LoadMenu;
#if UNITY_EDITOR
		_coldStartupLocation.OnLoadingRequested -= LocationColdStartup;
#endif
	}

#if UNITY_EDITOR
	/// <summary>
	/// This special loading function is only used in the editor, when the developer presses Play in a Location scene, without passing by Initialisation.
	/// </summary>
	private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen)
	{
		_currentlyLoadedScene = currentlyOpenedLocation;

		if(_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
		{
			//Gameplay managers is loaded synchronously
			_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			_gameplayManagerLoadingOpHandle.WaitForCompletion();
			_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

			StartGameplay();
		}
	}
#endif

	/// <summary>
	/// This function loads the location scenes passed as array parameter
	/// </summary>
	private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen)
	{
		_sceneToLoad = locationToLoad;
		_showLoadingScreen = showLoadingScreen;

		//In case we are coming from the main menu, we need to load the Gameplay manager scene first
		if (_gameplayManagerSceneInstance.Scene == null
			|| !_gameplayManagerSceneInstance.Scene.isLoaded)
		{
			_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			_gameplayManagerLoadingOpHandle.Completed += OnGameplayMangersLoaded;
		}
		else
		{
			UnloadPreviousScene();
		}
	}

	private void OnGameplayMangersLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

		UnloadPreviousScene();
	}

	/// <summary>
	/// Prepares to load the main menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
	/// </summary>
	private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen)
	{
		_sceneToLoad = menuToLoad;
		_showLoadingScreen = showLoadingScreen;

		//In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
		if (_gameplayManagerSceneInstance.Scene != null
			&& _gameplayManagerSceneInstance.Scene.isLoaded)
			Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);

		UnloadPreviousScene();
	}

	/// <summary>
	/// In both Location and Menu loading, this function takes care of removing previously loaded scenes.
	/// </summary>
	private void UnloadPreviousScene()
	{
		if(_currentlyLoadedScene != null) //would be null if the game was started in Initialisation
		{
			if(_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
			{
				//Unload the scene through its AssetReference, i.e. through the Addressable system
				_currentlyLoadedScene.sceneReference.UnLoadScene();
			}
#if UNITY_EDITOR
			else
			{
				//Only used when, after a "cold start", the player moves to a new scene
				//Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
				//the scene needs to be unloaded using regular SceneManager instead of as an Addressable
				SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
			}
#endif
		}

		LoadNewScene();
	}

	/// <summary>
	/// Kicks off the asynchronous loading of a scene, either menu or Location.
	/// </summary>
	private void LoadNewScene()
	{
		if (_showLoadingScreen)
		{
			_toggleLoadingScreen.RaiseEvent(true);
		}

		_loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
		_loadingOperationHandle.Completed += OnNewSceneLoaded;
	}

	private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		//Save loaded scenes (to be unloaded at next load request)
		_currentlyLoadedScene = _sceneToLoad;
		SetActiveScene();

		if (_showLoadingScreen)
		{
			_toggleLoadingScreen.RaiseEvent(false);
		}
	}

	/// <summary>
	/// This function is called when all the scenes have been loaded
	/// </summary>
	private void SetActiveScene()
	{
		Scene s = ((SceneInstance)_loadingOperationHandle.Result).Scene;
		SceneManager.SetActiveScene(s);

		LightProbes.TetrahedralizeAsync();

		StartGameplay();
	}

	private void StartGameplay()
	{
		_onSceneReady.RaiseEvent(); //Spawn system will spawn the PigChef
	}

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}
}
