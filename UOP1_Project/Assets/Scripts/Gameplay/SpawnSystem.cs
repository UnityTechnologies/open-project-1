using System;
using System.Linq;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
	[Header("Asset References")]

	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private Protagonist _playerPrefab = default;
	[SerializeField] private TransformAnchor _playerTransformAnchor = default;
	[SerializeField] private TransformEventChannelSO _playerInstantiatedChannel = default;
	[SerializeField] private PathStorageSO _pathTaken = default;

	[Header("Scene References")]
	private LocationEntrance[] _spawnLocations;
	private Transform _defaultSpawnPoint;

	[Header("Scene Ready Event")]
	[SerializeField] private VoidEventChannelSO _OnSceneReady = default; //Raised when the scene is loaded and set active

	private void Awake()
	{
		_spawnLocations = GameObject.FindObjectsOfType<LocationEntrance>();
		_defaultSpawnPoint = transform.GetChild(0);
	}

	private void OnEnable()
	{
		_OnSceneReady.OnEventRaised += SpawnPlayer;
	}

	private void OnDisable()
	{
		_OnSceneReady.OnEventRaised -= SpawnPlayer;
	}

	private Transform GetSpawnLocation()
	{
		if (_pathTaken == null)
			return _defaultSpawnPoint;

		//Look for the element in the available LocationEntries that matches tha last PathSO taken
		int entranceIndex = Array.FindIndex(_spawnLocations, element =>
			element.EntrancePath == _pathTaken.lastPathTaken );

		if (entranceIndex == -1)
		{
			Debug.LogWarning("The player tried to spawn in an LocationEntry that doesn't exist, returning the default one.");
			return _defaultSpawnPoint;
		}
		else
			return _spawnLocations[entranceIndex].transform;
	}

	private Protagonist InstantiatePlayer(Protagonist playerPrefab, Transform spawnLocation)
	{
		if (playerPrefab == null)
			throw new Exception("Player Prefab can't be null.");

		Protagonist playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);

		return playerInstance;
	}

	private void SpawnPlayer()
	{
		Protagonist playerInstance = InstantiatePlayer(_playerPrefab, GetSpawnLocation());

		_playerInstantiatedChannel.RaiseEvent(playerInstance.transform); // The CameraSystem will pick this up to frame the player
		_playerTransformAnchor.Transform = playerInstance.transform;

		//TODO: Probably move this to the GameManager once it's up and running
		_inputReader.EnableGameplayInput();
	}
}
