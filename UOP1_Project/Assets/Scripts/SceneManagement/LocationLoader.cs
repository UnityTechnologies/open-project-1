using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class manages the scenes loading and unloading
/// </summary>

public class LocationLoader : MonoBehaviour
{
	[Header("Initialization Scene")]
	public GameSceneSO initializationScene;
	[Header("Load on start")]
	public GameSceneSO[] mainMenuScenes;
	[Header("Loading Screen")]
	public GameObject loadingInterface;
	public Image loadingProgressBar;

	[Header("Load Event")]
	//The load event we are listening to
	[SerializeField] private LoadEvent _loadEvent = default;

	//List of the scenes to load and track progress
	private List<AsyncOperation> _scenesToLoadAsyncOperations = new List<AsyncOperation>();
	//List of scenes to unload
	private List<Scene> _ScenesToUnload = new List<Scene>();
	//Keep track of the scene we want to set as active (for lighting/skybox)
	private GameSceneSO _activeScene;

	private void OnEnable()
	{
		_loadEvent.loadEvent += LoadScenes;
	}

	private void OnDisable()
	{
		_loadEvent.loadEvent -= LoadScenes;
	}

	private void Start()
	{
		if (SceneManager.GetActiveScene().name == initializationScene.sceneName)
		{
			LoadMainMenu();
		}
	}

	private void LoadMainMenu()
	{
		LoadScenes(mainMenuScenes, false);
	}

	/// <summary> This function loads the scenes passed as array parameter </summary>
	public void LoadScenes(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		//Add all current open scenes to unload list
		AddScenesToUnload();

		_activeScene = locationsToLoad[0];

		for (int i = 0; i < locationsToLoad.Length; ++i)
		{
			String currentSceneName = locationsToLoad[i].sceneName;
			if (!CheckLoadState(currentSceneName))
			{
				//Add the scene to the list of scenes to load asynchronously in the background
				_scenesToLoadAsyncOperations.Add(SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive));
			}
		}
		_scenesToLoadAsyncOperations[0].completed += SetActiveScene;
		if (showLoadingScreen)
		{
			//Show the progress bar and track progress if loadScreen is true
			loadingInterface.SetActive(true);
			StartCoroutine(TrackLoadingProgress());
		}
		else
		{
			//Clear the scenes to load
			_scenesToLoadAsyncOperations.Clear();
		}

		//Unload the scenes
		UnloadScenes();
	}

	private void SetActiveScene(AsyncOperation asyncOp)
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(_activeScene.sceneName));
	}

	public void AddScenesToUnload()
	{
		for (int i = 0; i < SceneManager.sceneCount; ++i)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.name != initializationScene.sceneName)
			{
				Debug.Log("Added scene to unload = " + scene.name);
				//Add the scene to the list of the scenes to unload
				_ScenesToUnload.Add(scene);
			}
		}
	}

	public void UnloadScenes()
	{
		if (_ScenesToUnload != null)
		{
			for (int i = 0; i < _ScenesToUnload.Count; ++i)
			{
				//Unload the scene asynchronously in the background
				SceneManager.UnloadSceneAsync(_ScenesToUnload[i]);
			}
		}
		_ScenesToUnload.Clear();
	}

	/// <summary> This function checks if a scene is already loaded </summary>
	public bool CheckLoadState(String sceneName)
	{
		for (int i = 0; i < SceneManager.sceneCount; ++i)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.name == sceneName)
			{
				return true;
			}
		}
		return false;
	}

	/// <summary> This function updates the loading progress once per frame until loading is complete </summary>
	IEnumerator TrackLoadingProgress()
	{
		float totalProgress = 0;
		//When the scene reaches 0.9f, it means that it is loaded
		//The remaining 0.1f are for the integration
		while (totalProgress <= 0.9f)
		{

			//Reset the progress for the new values
			totalProgress = 0;
			//Iterate through all the scenes to load
			for (int i = 0; i < _scenesToLoadAsyncOperations.Count; ++i)
			{
				Debug.Log("Scene" + i + " :" + _scenesToLoadAsyncOperations[i].isDone + "progress = " + _scenesToLoadAsyncOperations[i].progress);
				//Adding the scene progress to the total progress
				totalProgress += _scenesToLoadAsyncOperations[i].progress;
			}
			//The fillAmount for all scenes, so we devide the progress by the number of scenes to load
			loadingProgressBar.fillAmount = totalProgress / _scenesToLoadAsyncOperations.Count;
			Debug.Log("progress bar" + loadingProgressBar.fillAmount + "and value =" + totalProgress / _scenesToLoadAsyncOperations.Count);

			yield return null;
		}

		//Clear the scenes to load
		_scenesToLoadAsyncOperations.Clear();

		//Hide progress bar when loading is done
		loadingInterface.SetActive(false);
	}

	public void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}

}
