using UnityEngine;

/// <summary>
/// This class goes on a trigger which, when entered, sends the player to another Location
/// </summary>
public class LocationExit : MonoBehaviour
{
	[SerializeField] private GameSceneSO _locationToLoad = default;
	[SerializeField] private PathSO _leadsToPath = default;
	[SerializeField] private PathStorageSO _pathStorage = default; //This is where the last path taken will be stored

	[Header("Broadcasting on")]
	[SerializeField] private LoadEventChannelSO _locationExitLoadChannel = default;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_pathStorage.lastPathTaken = _leadsToPath;
			_locationExitLoadChannel.RaiseEvent(_locationToLoad, false, true);
		}
	}
}
