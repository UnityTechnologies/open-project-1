using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnSystem : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private int _defaultSpawnIndex = 0;

	[Header("Asset References")]
	[SerializeField] private Protagonist _playerPrefab = default;
	[SerializeField] private TransformEventChannelSO _playerInstantiatedChannel = default;

	[Header("Scene References")]
	[SerializeField] private Transform[] _spawnLocations;

	void Start()
	{
		Spawn(_defaultSpawnIndex);
	}

	void Reset()
	{
		AutoFill();
	}

	/// <summary>
	/// This function tries to autofill some of the parameters of the component, so it's easy to drop in a new scene
	/// </summary>
	[ContextMenu("Attempt Auto Fill")]
	private void AutoFill()
	{
		if (_spawnLocations == null || _spawnLocations.Length == 0)
			_spawnLocations = transform.GetComponentsInChildren<Transform>(true)
								.Where(t => t != this.transform)
								.ToArray();
	}

	private void Spawn(int spawnIndex)
	{
		Transform spawnLocation = GetSpawnLocation(spawnIndex, _spawnLocations);
		Protagonist playerInstance = InstantiatePlayer(_playerPrefab, spawnLocation);

		_playerInstantiatedChannel.RaiseEvent(playerInstance.transform); // The CameraSystem will pick this up to frame the player
	}

	private Transform GetSpawnLocation(int index, Transform[] spawnLocations)
	{
		if (spawnLocations == null || spawnLocations.Length == 0)
			throw new Exception("No spawn locations set.");

		index = Mathf.Clamp(index, 0, spawnLocations.Length - 1);
		return spawnLocations[index];
	}

	private Protagonist InstantiatePlayer(Protagonist playerPrefab, Transform spawnLocation)
	{
		if (playerPrefab == null)
			throw new Exception("Player Prefab can't be null.");

		Protagonist playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);

		return playerInstance;
	}
}
