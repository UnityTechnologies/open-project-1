using UnityEngine;

/// <summary>
/// Spawns a prefab at its own location
/// </summary>
public class PrefabSpawnPoint : MonoBehaviour
{
    [SerializeField]
    [Tooltip("If True, the prefab will be spawned on Awake")]
    bool spawnOnAwake = true;
    [SerializeField]
    [Tooltip("If True, this GO will be parented in the spawned prefab")]
    bool parentUnderSpawnedObject = true;
    [SerializeField]
    [Tooltip("The Prefab of the spawn")]
    GameObject prefabToSpawn;

    /// <summary>
    /// Gather external components. Throws if it can't find them.
    /// </summary>
    void Awake()
    {
        if (prefabToSpawn == null)
            Debug.LogError("No prefab to spawn");

        if (spawnOnAwake)
            SpawnPrefab();
    }

    /// <summary>
    /// Spawns the prefab.
    /// </summary>
    public void SpawnPrefab()
    {
        var player = Instantiate(prefabToSpawn, transform.position, transform.rotation);
        if (parentUnderSpawnedObject)
            transform.parent = player.transform;
    }
}
