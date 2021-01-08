using UnityEngine;

/// <summary>
/// This class manages the case we are in editor and we want to press play from any scene
/// It takes care of telling the SpawnSystem that the scene is ready since we pressed play from it
/// and it is therefore already loaded
/// </summary> 
public class SceneReadyBroadcaster : MonoBehaviour
{
#if UNITY_EDITOR

	[Header("On Editor Initializer event")]
	[SerializeField] private VoidEventChannelSO _OnEditorInitializer = default;


	[Header("Broadcasting on")]
	[SerializeField] private VoidEventChannelSO _OnSceneReady = default;

	private void OnEnable()
	{
		if (_OnEditorInitializer != null)
		{
			_OnEditorInitializer.OnEventRaised += InformSceneIsReady;
		}
	}

	private void OnDisable()
	{
		if (_OnEditorInitializer != null)
		{
			_OnEditorInitializer.OnEventRaised -= InformSceneIsReady;
		}
	}

	void InformSceneIsReady()
	{
		_OnSceneReady.RaiseEvent();
	}
#endif
}
