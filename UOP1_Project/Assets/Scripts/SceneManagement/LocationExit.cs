using UnityEngine;

/// <summary>
/// This class goes on a trigger which, when entered, sends the player to another Location
/// </summary>

public class LocationExit : MonoBehaviour
{
	[SerializeField] private LoadEventChannelSO _onLocationExit = default;
	[SerializeField] private GameSceneSO[] _locationsToLoad = default;
	[SerializeField] private bool _showLoadScreen = default;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_onLocationExit.RaiseEvent(_locationsToLoad, _showLoadScreen);
		}
	}
}
