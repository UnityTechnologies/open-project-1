using UnityEngine;

/// <summary>
/// Spawns the player at its own location
/// </summary>
public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField]
    [Tooltip("If True, the player will be spawned on Awake")]
    bool spawnOnAwake = true;
    [SerializeField]
    [Tooltip("The Prefab of the player (Pig)")]
    GameObject playerPrefab;

    Transform gameplayCamera;
    InputReader inputReader;

    /// <summary>
    /// Gather external components. Throws if it can't find them.
    /// </summary>
    void Awake()
    {
        if (playerPrefab == null)
            Debug.LogError("No Player to spawn");

        inputReader = FindObjectOfType<InputReader>();
        if (inputReader == null)
            Debug.LogError("No Input Reader has been given to assign to the player");

        gameplayCamera = Camera.main.transform;
        if (gameplayCamera == null)
            Debug.LogError("No Gameplay Camera has been given to assign to the player");

        if (spawnOnAwake)
            SpawnPlayer();
    }

    /// <summary>
    /// Spawns the player and puts this GO as child of the newly spawned player.
    /// </summary>
    public void SpawnPlayer()
    {
        var player = Instantiate(playerPrefab, transform.position, transform.rotation);
        transform.parent = player.transform;
        var protagonistComponent = player.GetComponent<Protagonist>();
        protagonistComponent.Initialise(inputReader, gameplayCamera);
    }
}
