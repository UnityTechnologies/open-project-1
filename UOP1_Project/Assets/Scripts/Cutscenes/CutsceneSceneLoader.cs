using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSceneLoader : MonoBehaviour
{
	[SerializeField] private GameSceneSO _sceneToLoad = default;
	[Header("Broadcasting on")]
	[SerializeField] private LoadEventChannelSO _sceneLoadChannel = default;

	//Used to load a location from a custscene
	public void LoadScene()
	{
		_sceneLoadChannel.RaiseEvent(_sceneToLoad, false, true);
	}
}
