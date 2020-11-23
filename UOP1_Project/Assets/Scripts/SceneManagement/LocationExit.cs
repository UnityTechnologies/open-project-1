using UnityEngine;

/// <summary>
/// This class goes on a trigger which, when entered, sends the player to another Location
/// </summary>

public class LocationExit : MonoBehaviour
{
	[Header("Loading settings")]
	[SerializeField] private GameSceneSO[] _locationsToLoad = default;
	[SerializeField] private bool _showLoadScreen = default;

	[Header("Broadcasting on")]
	[SerializeField] private LoadEventChannelSO _locationExitLoadChannel = default;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_locationExitLoadChannel.RaiseEvent(_locationsToLoad, _showLoadScreen);
		}
	}
}
