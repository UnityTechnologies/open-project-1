using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class manages the scene loading and unloading.
/// Heavy on comments right now because it is still being worked on.
/// </summary>
public class LocationLoader : MonoBehaviour
{
	[Header("Initialization Scene")]
	[SerializeField] private GameSceneSO _initializationScene = default;

	[Header("Load on start")]
	[SerializeField] private GameSceneSO[] _mainMenuScenes = default;

	[Header("Loading Screen")]
	[SerializeField] private GameObject _loadingInterface = default;
	[SerializeField] private Image _loadingProgressBar = default;

	[Header("Load Event")]
	//The load event we are listening to
	[SerializeField] private LoadEventChannelSO _loadEventChannel = default;

	private List<AsyncOperation> _scenesToLoadAsyncOperations = new List<AsyncOperation>();
	private List<Scene> _scenesToUnload = new List<Scene>();
	private GameSceneSO _activeScene; // The scene we want to set as active (for lighting/skybox)

	private Coroutine runningLoader = null;

	private void OnEnable()
	{
		_loadEventChannel.OnLoadingRequested += LoadScenes;
	}

	private void OnDisable()
	{
		_loadEventChannel.OnLoadingRequested -= LoadScenes;
	}

	private void Start()
	{
		if (SceneManager.GetActiveScene().name == _initializationScene.sceneName)
		{
			LoadMainMenu();
		}
	}

	private void LoadMainMenu()
	{
		LoadScenes(_mainMenuScenes, false);
	}

	/// <summary>
	/// This function loads the scenes passed as array parameter 
	/// </summary>
	/// <param name="locationsToLoad"></param>
	/// <param name="showLoadingScreen"></param>
	private void LoadScenes(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		_activeScene = locationsToLoad[0];
		AddScenesToUnload();
		UnloadScenes();

		if (showLoadingScreen)
		{
			_loadingInterface.SetActive(true);
		}

		if (_scenesToLoadAsyncOperations.Count == 0)
		{
			for (int i = 0; i < locationsToLoad.Length; i++)
			{
				string currentSceneName = locationsToLoad[i].sceneName;
				if (IsSceneLoaded(currentSceneName) == false)
				{
					if (runningLoader == null)
					{
						_scenesToLoadAsyncOperations.Add(SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive));
						_scenesToLoadAsyncOperations[i].completed += SetActiveScene;
						// TODO: Run a coroutine for each scene loading that updates a combined value
						// for the progress bar. This way, as each scene completes loading, we will
						// know what scene it is. Then decide if it activates right away or not.
						// runningLoader = StartCoroutine(TrackLoadingProgress(locationsToLoad[i]));
					}
				}
			}

			if (_scenesToLoadAsyncOperations.Count > 0)
			{
				// TODO: locationsToLoad[0] is a place holder right now.
				runningLoader = StartCoroutine(TrackLoadingProgress(locationsToLoad[0]));
			}
		}
	}

	/// <summary>
	/// SetActiveScene(AsyncOperation asyncOp) is called by AsyncOperation.complete event.
	/// </summary>
	/// <param name="asyncOp"></param>
	private void SetActiveScene(AsyncOperation asyncOp)
	{
		// TODO: As each event completes, decide if it needs to activate right away.
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(_activeScene.sceneName));
	}

	private void AddScenesToUnload()
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.name != _initializationScene.sceneName && scene.name != _activeScene.name)
			{
				Debug.Log("Added scene to unload = " + scene.name);
				_scenesToUnload.Add(scene);
			}
		}
	}

	private void UnloadScenes()
	{
		if (_scenesToUnload != null)
		{
			for (int i = 0; i < _scenesToUnload.Count; i++)
			{
				SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
			}
			_scenesToUnload.Clear();
		}
	}

	/// <summary>
	/// This function checks if a scene is already loaded
	/// </summary>
	/// <param name="sceneName"></param>
	/// <returns>bool</returns>
	private bool IsSceneLoaded(string sceneName)
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.name == sceneName)
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	///  This function updates the loading progress once per frame until loading is complete 
	/// </summary>
	/// <param name="sceneReference">This is a place holder for the moment</param>
	/// <returns>IEnumerator</returns>
	private IEnumerator TrackLoadingProgress(GameSceneSO sceneReference)
	{
		float totalProgress = 0;
		// When the scene reaches 0.9f, it means that it is loaded
		// The remaining 0.1f are for the integration
		while (totalProgress <= 0.9f)
		{

			totalProgress = 0;
			for (int i = 0; i < _scenesToLoadAsyncOperations.Count; ++i)
			{
				Debug.Log("Scene " + i + " :" + _scenesToLoadAsyncOperations[i].isDone + " progress = " + _scenesToLoadAsyncOperations[i].progress);
				totalProgress += _scenesToLoadAsyncOperations[i].progress;
			}

			//The fillAmount is for all scenes, so we divide the progress by the number of scenes to load
			_loadingProgressBar.fillAmount = totalProgress / _scenesToLoadAsyncOperations.Count;
			Debug.Log("progress bar " + _loadingProgressBar.fillAmount + " and value = " + totalProgress / _scenesToLoadAsyncOperations.Count);

			yield return null;
		}

		_scenesToLoadAsyncOperations.Clear();

		runningLoader = null;

		//Hide progress bar when loading is done
		_loadingInterface.SetActive(false);
	}

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}

}
