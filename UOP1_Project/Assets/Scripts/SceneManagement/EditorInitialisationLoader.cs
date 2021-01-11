using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script loads the persistent managers and gameplay Scenes, to allow to start the game from any gameplay Scene
/// It can also be used for menu scene by just adding the persistent managers scene on the inspector
/// </summary>
public class EditorInitialisationLoader : MonoBehaviour
{
#if UNITY_EDITOR
	public GameSceneSO[] scenesToLoad;
	public int targetFramerate = 0; // For debugging purposes
	//bool to know if we are coming from editor initializer mode
	[HideInInspector] public bool _isEditorInitializerMode=false;

	private void Start()
	{
		Application.targetFrameRate = targetFramerate; // For debugging purposes

		for (int i = 0; i < SceneManager.sceneCount; ++i)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			for (int j = 0; j < scenesToLoad.Length; ++j)
				if (scene.path == scenesToLoad[j].scenePath)
				{
					return;
				}
				else
				{
					SceneManager.LoadSceneAsync(scenesToLoad[j].scenePath, LoadSceneMode.Additive);
					//Inform that we are pressing play from a location or menu
					_isEditorInitializerMode = true;
				}
		}
	}
#endif
}
