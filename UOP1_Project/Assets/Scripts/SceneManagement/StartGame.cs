using UnityEngine;

/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>

public class StartGame : MonoBehaviour
{
	[SerializeField] private LoadSceneChannelSO _onPlayButtonPress;
	[SerializeField] private LocationSelection _locationToLoad;
	public bool showLoadScreen;

	public void OnPlayButtonPress()
	{
		_onPlayButtonPress.RaiseEvent(_locationToLoad, showLoadScreen);
	}
}
