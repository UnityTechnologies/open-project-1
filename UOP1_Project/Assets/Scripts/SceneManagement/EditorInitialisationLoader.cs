using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script loads the persistent managers and gameplay Scenes, to allow to start the game from any gameplay Scene
/// It can also be used for menu scene by just adding the persistent managers scene on the inspector
/// </summary>
public class EditorInitialisationLoader : MonoBehaviour
{
#if UNITY_EDITOR
	public int targetFramerate = 0; // For debugging purposes
	//bool to know if we are coming from editor initializer mode
	[HideInInspector] public bool _shootEvent = true;
#endif
}
