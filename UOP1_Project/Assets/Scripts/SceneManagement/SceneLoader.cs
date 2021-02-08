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
	[SerializeField] private GameSceneSO _persistentManagersScene = default;
	[SerializeField] private GameSceneSO _gameplayScene = default;

	[Header("Load Events")]
	[SerializeField] private LoadEventChannelSO _loadLocation = default;
	[SerializeField] private LoadEventChannelSO _loadMenu = default;

	[Header("Broadcasting on")]
	[SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;
	[SerializeField] private FadeChannelSO _onFade = default;

	private List<AsyncOperationHandle<SceneInstance>> _temporarySceneLoadOpHandles = new List<AsyncOperationHandle<SceneInstance>>();
	private AsyncOperationHandle<SceneInstance> _gameplayManagerSceneLoadOpHandle = new AsyncOperationHandle<SceneInstance>();
	private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();

	private GameSceneSO _activeScene; // The scene we want to set as active (for lighting/skybox)

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
		//In case we are coming from the main menu, we need to get rid of the persistent Gameplay manager scene
		if (_gameplayManagerSceneInstance.Scene.isLoaded)
			Addressables.LoadSceneAsync(_gameplayManagerSceneLoadOpHandle,
													LoadSceneMode.Additive, true).Completed += GameplayManagerLoaded;

		UnloadPreviousScenes();
		LoadScenes(locationsToLoad, showLoadingScreen);
	}

	private void GameplayManagerLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		_gameplayManagerSceneInstance = (SceneInstance)obj.Result;
	}

	/// <summary>
	/// This function loads the menu scenes passed as array parameter 
	/// </summary>
	private void LoadMenu(GameSceneSO[] menuToLoad, bool showLoadingScreen)
	{
		//In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
		if(_gameplayManagerSceneInstance.Scene.isLoaded)
			Addressables.UnloadSceneAsync(_gameplayManagerSceneLoadOpHandle, true);

		UnloadPreviousScenes();
		LoadScenes(menuToLoad, showLoadingScreen);
	}

	private void UnloadPreviousScenes()
	{
		for (int i = 0; i < _temporarySceneLoadOpHandles.Count; i++)
		{
			Addressables.UnloadSceneAsync(_temporarySceneLoadOpHandles[i], true);
		}
	}

	private void LoadScenes(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		//Take the first scene in the array as the scene we want to set active
		_activeScene = locationsToLoad[0];

		if (showLoadingScreen)
		{
			_toggleLoadingScreen.RaiseEvent(true);
		}

		for (int i = 0; i < locationsToLoad.Length; i++)
		{
			_temporarySceneLoadOpHandles.Add(locationsToLoad[i].sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0));
		}

		StartCoroutine(LoadingProcess(showLoadingScreen));
	}

	private IEnumerator LoadingProcess(bool showLoadingScreen)
	{
		bool _loadingDone = false;

		//This while will exit when all scenes requested have been loaded
		while (!_loadingDone)
		{
			for (int i = 0; i < _temporarySceneLoadOpHandles.Count; ++i)
			{
				if (_temporarySceneLoadOpHandles[i].Status != AsyncOperationStatus.Succeeded)
				{
					break;
				}
				else
				{
					_loadingDone = true;
				}
			}

			yield return null;
		}

		SetActiveScene();

		if (showLoadingScreen)
		{
			_toggleLoadingScreen.RaiseEvent(false);
		}

	}

	/// <summary>
	/// This function is called when all the scenes have been loaded
	/// </summary>
	private void SetActiveScene()
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_activeScene.scenePath));

		// Will reconstruct LightProbe tetrahedrons to include the probes from the newly-loaded scene
		LightProbes.TetrahedralizeAsync();

		_onSceneReady.RaiseEvent();
	}

	/// <summary>
	/// This function checks if a scene is already loaded
	/// </summary>
	/// <param name="scenePath"></param>
	/// <returns>bool</returns>
	private bool IsSceneLoaded(string scenePath)
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.path == scenePath)
			{
				return true;
			}
		}
		return false;
	}

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}
}
