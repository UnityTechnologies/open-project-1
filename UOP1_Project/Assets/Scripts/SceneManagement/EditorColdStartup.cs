using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// Allows a "cold start" in the editor, when pressing Play and not passing from the Initialisation scene.
/// </summary> 
public class EditorColdStartup : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] private GameSceneSO _thisSceneSO = default;
	[SerializeField] private GameSceneSO _persistentManagersSO = default;
	[SerializeField] private AssetReference _notifyColdStartupChannel = default;
	[SerializeField] private VoidEventChannelSO _onSceneReadyChannel = default;
	[SerializeField] private PathStorageSO _pathStorage = default;
	[SerializeField] private SaveSystem _saveSystem = default;

	private bool isColdStart = false;
	private void Awake()
	{
		if (!SceneManager.GetSceneByName(_persistentManagersSO.sceneReference.editorAsset.name).isLoaded)
		{
			isColdStart = true;

			//Reset the path taken, so the character will spawn in this location's default spawn point
			_pathStorage.lastPathTaken = null;
		}
		CreateSaveFileIfNotPresent();
	}

	private void Start()
	{
		if (isColdStart)
		{
			_persistentManagersSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;

		}
		CreateSaveFileIfNotPresent();
	}
	private void CreateSaveFileIfNotPresent()
	{
		if (_saveSystem != null && !_saveSystem.LoadSaveDataFromDisk())
		{
			_saveSystem.SetNewGameData();
		}
	}
	private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
	{
		_notifyColdStartupChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += OnNotifyChannelLoaded;
	}

	private void OnNotifyChannelLoaded(AsyncOperationHandle<LoadEventChannelSO> obj)
	{
		if (_thisSceneSO != null)
		{
			obj.Result.RaiseEvent(_thisSceneSO);
		}
		else
		{
			//Raise a fake scene ready event, so the player is spawned
			_onSceneReadyChannel.RaiseEvent();
			//When this happens, the player won't be able to move between scenes because the SceneLoader has no conception of which scene we are in
		}
	}


#endif
}
