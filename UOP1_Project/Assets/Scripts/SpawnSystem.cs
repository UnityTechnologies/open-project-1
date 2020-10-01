using System;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerSpawnEvent : UnityEvent<Transform> { }

public class SpawnSystem : MonoBehaviour
{
    private static int requestedSpawnIndex = -1;

    [Header("Settings")]
    [SerializeField] private int defaultSpawnIndex = 0;

    [Header("Project References")]
#pragma warning disable 649
    [SerializeField] private Protagonist playerPrefab;
#pragma warning restore 649

    [Header("Scene References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform gameplayCamera;
    [SerializeField] private Transform[] spawnLocations;

    [Header("Events")]
    public PlayerSpawnEvent onPlayerSpawnEvent;

    void Awake()
    {
        try
        {
            Spawn();
        }
        catch (Exception e)
        {
            Debug.LogError($"[SpawnSystem] Failed to spawn player. {e.Message}");
        }
    }

    void Reset()
    {
        AutoFill();
    }

    [ContextMenu("Attempt Auto Fill")]
    private void AutoFill()
    {
        if (inputReader == null)
            inputReader = FindObjectOfType<InputReader>();

        if (gameplayCamera == null)
            gameplayCamera = GameObject.FindGameObjectWithTag("MainCamera")?.transform;

        if (spawnLocations == null || spawnLocations.Length == 0)
            spawnLocations = transform.GetComponentsInChildren<Transform>(true)
                                .Where(t => t != this.transform)
                                .ToArray();
    }

    private void Spawn()
    {
        int spawnIndex = requestedSpawnIndex < 0 ? defaultSpawnIndex : requestedSpawnIndex;
        var spawnLocation = GetSpawnLocation(spawnIndex, spawnLocations);
        var playerInstance = InstantiatePlayer(playerPrefab, spawnLocation, inputReader, gameplayCamera);
        requestedSpawnIndex = -1;

        onPlayerSpawnEvent.Invoke(playerInstance.transform);
    }

    private static Transform GetSpawnLocation(int index, Transform[] spawnLocations)
    {
        if (spawnLocations == null || spawnLocations.Length == 0)
            throw new Exception("No spawn locations set.");

        index = Mathf.Clamp(index, 0, spawnLocations.Length - 1);
        return spawnLocations[index];
    }

    private static Protagonist InstantiatePlayer(Protagonist playerPrefab, Transform spawnLocation, InputReader inputReader, Transform gameplayCamera)
    {
        if (playerPrefab == null)
            throw new Exception("Player Prefab can't be null.");

        bool originalState = playerPrefab.enabled;
        // Prevents playerInstance's Protagonist.OnEnable from running before we set the inputReader and gameplayCamera references
        playerPrefab.enabled = false;
        var playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        playerPrefab.enabled = originalState;

        playerInstance.inputReader = inputReader;
        playerInstance.gameplayCamera = gameplayCamera;
        // Since the prefab's script was disabled it need to be enabled here
        playerInstance.enabled = true;
        return playerInstance;
    }

    public static void SetRequestSpawnIndex(int spawnIndex)
    {
        // Prevent setting negative numbers from external sources
        requestedSpawnIndex = Mathf.Max(spawnIndex, 0);
    }
}