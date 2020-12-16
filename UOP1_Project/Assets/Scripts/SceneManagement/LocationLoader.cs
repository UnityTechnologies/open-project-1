using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class manages the scene loading and unloading.
/// </summary>

public class LocationLoader : MonoBehaviour
{
	[Tooltip("Stores the spawn point location for the spawn system to use.")]
	[SerializeField] private SpawnLocationSO _spawnPoint;
	[SerializeField] private LocationSelection _initializationScene;

	[SerializeField] private LocationSelection _mainMenuScene;

	[Header("Load Event")]
	[SerializeField] private LoadSceneChannelSO _loadSceneChannel = default;
	[Header("Move Player Event")]
	[SerializeField] private Vector3ArrayChannelSO _movePlayerChannel = default;

	private AsyncOperation _loadingOperation;
	private List<Scene> _scenesToUnload = new List<Scene>();
	private SceneSO _activeScene;


	private Coroutine runningLoader = null;

	private void OnEnable()
	{
		if (_loadSceneChannel != null)
			_loadSceneChannel.OnLoadingRequested += LoadScene;
	}
	private void OnDisable()
	{
		if (_loadSceneChannel != null)
			_loadSceneChannel.OnLoadingRequested -= LoadScene;
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			LoadScene(_mainMenuScene, false);
		}
	}

	private void LoadScene(LocationSelection selection, bool showLoadingScreen)
	{
		if (selection != null)
		{
			_activeScene = SceneDatabaseSO.Instance.Scenes[selection.SceneIndex];
			AddScenesToUnload();
			UnloadScenes();
			_spawnPoint.Location = selection;
			string currentSceneName = _activeScene.SceneName;
			if (IsSceneLoaded(currentSceneName) == false)
			{
					_loadingOperation = SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
					_loadingOperation.completed += SetActiveScene;
			}
			else
			{
				_movePlayerChannel.RaiseEvent(_spawnPoint.GetVectors());
			}
		}
		else
		{
			Debug.Log("Selection is null");
		}
	}
	/// <summary>
	/// SetActiveScene(AsyncOperation asyncOp) is called by AsyncOperation.complete event.
	/// </summary>
	/// <param name="asyncOp"></param>
	private void SetActiveScene(AsyncOperation asyncOp)
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(_activeScene.SceneName));
	}

	private void AddScenesToUnload()
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.name != SceneDatabaseSO.Instance.Scenes[_initializationScene.SceneIndex].SceneName && scene.name != _activeScene.name)
			{
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

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}

}
