using UnityEngine;

/// <summary>
/// This class contains the function to call when play button is pressed
/// </summary>

public class StartGame : MonoBehaviour
{
	public LoadEventChannelSO onPlayButtonPress;
	[SerializeField] private SpawnPointSO _spawnPoint;
	public GameSceneSO[] locationsToLoad;
	[SerializeField] private PointSO _destination;
	public bool showLoadScreen;

	public void OnPlayButtonPress()
	{
		_spawnPoint.ExecuteLoad(_destination, locationsToLoad[0]);
		onPlayButtonPress.RaiseEvent(locationsToLoad, showLoadScreen);
	}
}
