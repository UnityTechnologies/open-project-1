using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class contains method to allow starting the game from any scene
/// </summary>

public class LoadSceneLoader : MonoBehaviour
{
    void Start()
    {
		#if UNITY_EDITOR
		if (SceneManager.sceneCount > 0)
		{
			for (int i = 0; i < SceneManager.sceneCount; ++i)
			{
				Scene scene = SceneManager.GetSceneAt(i);
				if (scene.name == "ScenesLoader")
				{
					return;
				}
			}
			SceneManager.LoadSceneAsync("ScenesLoader", LoadSceneMode.Additive);
		}
		#endif
	}
}
