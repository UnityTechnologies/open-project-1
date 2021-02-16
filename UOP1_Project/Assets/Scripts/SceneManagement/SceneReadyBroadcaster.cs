using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the case we are in editor and we want to press play from any scene
/// It takes care of telling the SpawnSystem that the scene is ready since we pressed play from it
/// and it is therefore already loaded
/// </summary> 
public class SceneReadyBroadcaster : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;
#if UNITY_EDITOR
	[Header("Editor cold-startup settings")]
	[SerializeField] private GameSceneSO _thisSceneSO = default;
	[SerializeField] private GameSceneSO _persistentManagersScene = default;
	[SerializeField] private LoadEventChannelSO _loadSceneEventChannel = default;
#endif

	private void Start()
	{
#if UNITY_EDITOR
		_persistentManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += ReloadScene;
#endif

		_onSceneReady.RaiseEvent();
	}

	private void ReloadScene(AsyncOperationHandle<SceneInstance> obj)
	{
		_loadSceneEventChannel.RaiseEvent(new GameSceneSO[] { _thisSceneSO });

		SceneManager.UnloadSceneAsync(_thisSceneSO.sceneReference.editorAsset.name);
	}
}
