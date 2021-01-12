using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scene loading and unloading.
/// </summary>
public class SceneLoader : MonoBehaviour
{
	[Header("Persistent Manager Scene")]
	[SerializeField] private GameSceneSO _persistentManagersScene = default;

	[Header("Gameplay Scene")]
	[SerializeField] private GameSceneSO _gameplayScene = default;

	[Header("Load Events")]
	//The location load event we are listening to
	[SerializeField] private LoadEventChannelSO _loadLocation = default;
	//The menu load event we are listening to
	[SerializeField] private LoadEventChannelSO _loadMenu = default;

	[Header("Broadcasting on")]
	[SerializeField] private BoolEventChannelSO _ToggleLoadingScreen = default;
	[SerializeField] private VoidEventChannelSO _OnSceneReady = default;

	private List<AsyncOperation> _scenesToLoadAsyncOperations = new List<AsyncOperation>();
	private List<Scene> _scenesToUnload = new List<Scene>();
	private GameSceneSO _activeScene; // The scene we want to set as active (for lighting/skybox)
	private List<GameSceneSO> _persistentScenes = new List<GameSceneSO>(); //Scenes to keep loaded when a load event is raised

	private void OnEnable()
	{
		if (_loadLocation != null)
		{
			_loadLocation.OnLoadingRequested += LoadLocation;
		}
		if (_loadMenu != null)
		{
			_loadMenu.OnLoadingRequested += LoadMenu;
		}
	}

	private void OnDisable()
	{
		if (_loadLocation != null)
		{
			_loadLocation.OnLoadingRequested -= LoadLocation;
		}
		if (_loadMenu != null)
		{
			_loadMenu.OnLoadingRequested -= LoadMenu;
		}
	}

	/// <summary>
	/// This function loads the location scenes passed as array parameter 
	/// </summary>
	/// <param name="locationsToLoad"></param>
	/// <param name="showLoadingScreen"></param>
	private void LoadLocation(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		//When loading a location, we want to keep the persistent managers and gameplay scenes loaded
		_persistentScenes.Add(_persistentManagersScene);
		_persistentScenes.Add(_gameplayScene);
		AddScenesToUnload(_persistentScenes);
		LoadScenes(locationsToLoad, showLoadingScreen);
	}

	/// <summary>
	/// This function loads the menu scenes passed as array parameter 
	/// </summary>
	/// <param name="MenuToLoad"></param>
	/// <param name="showLoadingScreen"></param>
	private void LoadMenu(GameSceneSO[] MenuToLoad, bool showLoadingScreen)
	{
		//When loading a menu, we only want to keep the persistent managers scene loaded
		_persistentScenes.Add(_persistentManagersScene);
		AddScenesToUnload(_persistentScenes);
		LoadScenes(MenuToLoad, showLoadingScreen);
	}

	private void LoadScenes(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		//Take the first scene in the array as the scene we want to set active
		_activeScene = locationsToLoad[0];
		UnloadScenes();

		if (showLoadingScreen)
		{
			_ToggleLoadingScreen.RaiseEvent(true);
		}

		if (_scenesToLoadAsyncOperations.Count == 0)
		{
			for (int i = 0; i < locationsToLoad.Length; i++)
			{
				string currentScenePath = locationsToLoad[i].scenePath;
				_scenesToLoadAsyncOperations.Add(SceneManager.LoadSceneAsync(currentScenePath, LoadSceneMode.Additive));
			}
		}

		//Checks if any of the persistent scenes is not loaded yet and load it if unloaded
		//This is especially useful when we go from main menu to first location
		for (int i = 0; i < _persistentScenes.Count; ++i)
		{
			if (IsSceneLoaded(_persistentScenes[i].scenePath) == false)
			{
				_scenesToLoadAsyncOperations.Add(SceneManager.LoadSceneAsync(_persistentScenes[i].scenePath, LoadSceneMode.Additive));
			}
		}
		StartCoroutine(WaitForLoading(showLoadingScreen));
	}

	private IEnumerator WaitForLoading(bool showLoadingScreen)
	{
		bool _loadingDone = false;
		// Wait until all scenes are loaded
		while (!_loadingDone)
		{
			for (int i = 0; i < _scenesToLoadAsyncOperations.Count; ++i)
			{
				if (!_scenesToLoadAsyncOperations[i].isDone)
				{
					break;
				}
				else
				{
					_loadingDone = true;
					_scenesToLoadAsyncOperations.Clear();
					_persistentScenes.Clear();
				}
			}
			yield return null;
		}
		//Set the active scene
		SetActiveScene();
		if (showLoadingScreen)
		{
			//Raise event to disable loading screen 
			_ToggleLoadingScreen.RaiseEvent(false);
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
		//Raise the event to inform that the scene is loaded and set active
		_OnSceneReady.RaiseEvent();
	}

	private void AddScenesToUnload(List<GameSceneSO> persistentScenes)
	{
		for (int i = 0; i < SceneManager.sceneCount; ++i)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			string scenePath = scene.path;
			for (int j = 0; j < persistentScenes.Count; ++j)
			{
				if (scenePath != persistentScenes[j].scenePath)
				{
					//Check if we reached the last persistent scenes check
					if (j == persistentScenes.Count - 1)
					{
						//If the scene is not one of the persistent scenes, we add it to the scenes to unload
						_scenesToUnload.Add(scene);
					}
				}
				else
				{
					//We move the next scene check as soon as we find that the scene is one of the persistent scenes
					break;
				}
			}
		}
	}

	private void UnloadScenes()
	{
		if (_scenesToUnload != null)
		{
			for (int i = 0; i < _scenesToUnload.Count; ++i)
			{
				SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
			}
			_scenesToUnload.Clear();
		}
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
