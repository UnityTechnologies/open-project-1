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

	[Header("Broadcasting on")]
	[SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

	private List<AsyncOperationHandle<SceneInstance>> _loadingOperationHandles = new List<AsyncOperationHandle<SceneInstance>>();
	private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

	//Parameters coming from scene loading requests
	private GameSceneSO[] _scenesToLoad;
	private GameSceneSO[] _currentlyLoadedScenes = new GameSceneSO[] { };
	private bool _showLoadingScreen;

	private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();

	private void OnEnable()
	{
		_loadLocation.OnLoadingRequested += LoadLocation;
		_loadMenu.OnLoadingRequested += LoadMenu;
	}

	private void OnDisable()
	{
		_loadLocation.OnLoadingRequested -= LoadLocation;
		_loadMenu.OnLoadingRequested -= LoadMenu;
	}

	/// <summary>
	/// This function loads the location scenes passed as array parameter
	/// </summary>
	private void LoadLocation(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		_scenesToLoad = locationsToLoad;
		_showLoadingScreen = showLoadingScreen;

		//In case we are coming from the main menu, we need to load the persistent Gameplay manager scene first
		if (_gameplayManagerSceneInstance.Scene == null
			|| !_gameplayManagerSceneInstance.Scene.isLoaded)
		{
			StartCoroutine(ProcessGameplaySceneLoading(locationsToLoad, showLoadingScreen));
		}
		else
		{
			UnloadPreviousScenes();
		}
	}

	private IEnumerator ProcessGameplaySceneLoading(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);

		while (_gameplayManagerLoadingOpHandle.Status != AsyncOperationStatus.Succeeded)
		{
			yield return null;
		}
		_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

		UnloadPreviousScenes();
	}

	/// <summary>
	/// Prepares to load the main menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
	/// </summary>
	private void LoadMenu(GameSceneSO[] menusToLoad, bool showLoadingScreen)
	{
		_scenesToLoad = menusToLoad;
		_showLoadingScreen = showLoadingScreen;

		//In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
		if (_gameplayManagerSceneInstance.Scene != null
			&& _gameplayManagerSceneInstance.Scene.isLoaded)
			Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);

		UnloadPreviousScenes();
	}

	/// <summary>
	/// In both Location and Menu loading, this function takes care of removing previously loaded temporary scenes.
	/// </summary>
	private void UnloadPreviousScenes()
	{
		for (int i = 0; i < _currentlyLoadedScenes.Length; i++)
		{
			_currentlyLoadedScenes[i].sceneReference.UnLoadScene();
		}

		LoadNewScenes();
	}

	/// <summary>
	/// Kicks off the asynchronous loading of an array of scenes, either menus or Locations.
	/// </summary>
	private void LoadNewScenes()
	{
		if (_showLoadingScreen)
		{
			_toggleLoadingScreen.RaiseEvent(true);
		}

		_loadingOperationHandles.Clear();
		//Build the array of handles of the temporary scenes to load
		for (int i = 0; i < _scenesToLoad.Length; i++)
		{
			_loadingOperationHandles.Add(_scenesToLoad[i].sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0));
		}

		StartCoroutine(LoadingProcess());
	}

	private IEnumerator LoadingProcess()
	{
		bool done = _loadingOperationHandles.Count == 0;

		//This while will exit when all scenes requested have been unloaded
		while (!done)
		{
			for (int i = 0; i < _loadingOperationHandles.Count; ++i)
			{
				if (_loadingOperationHandles[i].Status != AsyncOperationStatus.Succeeded)
				{
					break;
				}
				else
				{
					done = true;
				}
			}

			yield return null;
		}

		//Save loaded scenes (to be unloaded at next load request)
		_currentlyLoadedScenes = _scenesToLoad;
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
		//All the scenes have been loaded, so we assume the first in the array is ready to become the active scene
		Scene s = ((SceneInstance)_loadingOperationHandles[0].Result).Scene;
		SceneManager.SetActiveScene(s);

		LightProbes.TetrahedralizeAsync();

		_onSceneReady.RaiseEvent(); //Spawn system will spawn the PigChef
	}

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}
}
