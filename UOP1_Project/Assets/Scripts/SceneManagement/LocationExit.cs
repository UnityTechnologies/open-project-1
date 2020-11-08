using UnityEngine;

/// <summary>
/// This class goes on a trigger which, when entered, sends the player to another Location
/// </summary>

public class LocationExit : MonoBehaviour
{
	public LoadEvent onLocationExit;
	public GameSceneSO[] locationsToLoad;
	public bool showLoadScreen;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			onLocationExit.Raise(locationsToLoad, showLoadScreen);
		}
	}
}
