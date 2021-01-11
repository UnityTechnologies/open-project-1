using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene 
/// and raising the event to load the Main Menu
/// </summary>

public class InitializationLoader : MonoBehaviour
{
	[Header("Persistent managers Scene")]
	[SerializeField] private GameSceneSO _PersistentManagersScene = default;

	[Header("Loading settings")]
	[SerializeField] private GameSceneSO[] _MenuToLoad = default;
	[SerializeField] private bool _showLoadScreen = default;

	[Header("Broadcasting on")]
	[SerializeField] private LoadEventChannelSO _MenuLoadChannel = default;

	void Start()
	{
		//Load the persistent managers scene
		StartCoroutine(loadScene(_PersistentManagersScene.scenePath));
	}

	IEnumerator loadScene(string scenePath)
	{
		AsyncOperation loadingSceneAsyncOp = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);

		//Wait until we are done loading the scene
		while (!loadingSceneAsyncOp.isDone)
		{
			yield return null;
		}
		//Raise the event to load the main menu
		_MenuLoadChannel.RaiseEvent(_MenuToLoad, _showLoadScreen);
	}
}
