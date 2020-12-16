using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnSystem : MonoBehaviour
{

	[Header("Asset References")]
	[SerializeField] private Protagonist _playerPrefab = default;
	[SerializeField] private TransformEventChannelSO _playerInstantiatedChannel = default;
	[SerializeField] private SpawnLocationSO _spawnLocation = default;

	void Start()
	{
		Spawn();
	}

	private void Spawn()
	{
		Vector3[] spawnPoint = _spawnLocation.GetVectors();
		Protagonist playerInstance = InstantiatePlayer(_playerPrefab, spawnPoint[0], spawnPoint[1]);

		_playerInstantiatedChannel.RaiseEvent(playerInstance.transform); // The CameraSystem will pick this up to frame the player
	}

	private Protagonist InstantiatePlayer(Protagonist playerPrefab, Vector3 spawnPosition, Vector3 spawnRotation)
	{
		if (playerPrefab == null)
			throw new Exception("Player Prefab can't be null.");

		Protagonist playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(spawnRotation));

		return playerInstance;
	}
}
