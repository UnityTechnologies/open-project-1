using UnityEngine;

public class CutsceneSceneLoader : MonoBehaviour
{
	[SerializeField] private GameSceneSO _sceneToLoad = default;

	[Header("Broadcasting on")]
	[SerializeField] private LoadEventChannelSO _sceneLoadChannel = default;

	//Used to load a location or menu from a cutscene
	public void LoadScene()
	{
		_sceneLoadChannel.RaiseEvent(_sceneToLoad, false, true);
	}
}
