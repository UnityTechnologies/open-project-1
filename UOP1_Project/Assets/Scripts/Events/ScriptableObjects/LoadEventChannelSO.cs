using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for scene-loading events.
/// Takes a GameSceneSO of the location or menu that needs to be loaded, and a bool to specify if a loading screen needs to display.
/// </summary>
[CreateAssetMenu(menuName = "Events/Load Event Channel")]
public class LoadEventChannelSO : DescriptionBaseSO
{
	public UnityAction<GameSceneSO, bool, bool> OnLoadingRequested;

	public void RaiseEvent(GameSceneSO locationToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
	{
		if (OnLoadingRequested != null)
		{
			OnLoadingRequested.Invoke(locationToLoad, showLoadingScreen, fadeScreen);
		}
		else
		{
			Debug.LogWarning("A Scene loading was requested, but nobody picked it up. " +
				"Check why there is no SceneLoader already present, " +
				"and make sure it's listening on this Load Event channel.");
		}
	}
}
