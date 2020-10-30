using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class contains method to allow starting the game from any scene
/// </summary>

public class LoadSceneLoader : MonoBehaviour
{
	public GameSceneSO initializationScene;
	void Start()
	{
#if UNITY_EDITOR
		for (int i = 0; i < SceneManager.sceneCount; ++i)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.name == initializationScene.sceneName)
			{
				return;
			}
		}
		SceneManager.LoadSceneAsync(initializationScene.sceneName, LoadSceneMode.Additive);
#endif
	}
}
