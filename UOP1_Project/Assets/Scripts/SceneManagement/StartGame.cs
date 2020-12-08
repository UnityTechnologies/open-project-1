using UnityEngine;

/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>

public class StartGame : MonoBehaviour
{
	public LoadEventChannelSO onPlayButtonPress;
	public GameSceneSO[] locationsToLoad;
	public bool showLoadScreen;

	public void OnPlayButtonPress()
	{
		onPlayButtonPress.RaiseEvent(locationsToLoad, showLoadScreen);
	}
}
