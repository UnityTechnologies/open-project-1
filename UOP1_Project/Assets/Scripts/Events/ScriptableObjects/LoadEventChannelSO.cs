using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is a used for scene loading events.
/// Takes an array of the scenes we want to load and a bool to specify if we want to show a loading screen.
/// </summary>
[CreateAssetMenu(menuName = "Events/Load Event Channel")]
public class LoadEventChannelSO : ScriptableObject
{
	public UnityAction<GameSceneSO[], bool> OnLoadingRequested;

	public void RaiseEvent(GameSceneSO[] locationsToLoad, bool showLoadingScreen)
	{
		if (OnLoadingRequested != null)
			OnLoadingRequested.Invoke(locationsToLoad, showLoadingScreen);
	}
}
