using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct SpawnData
{
    //I couldn't decide on whether to use build index or scene name. Please let me know if any change is needed either from GitHub or from emreb25@outlook.com
    [Tooltip("If game loads THIS scene during runtime, parameters you set below will be used for spawning.")]
    public string sceneName;

    [Tooltip("The spawn position of the character.")]
    public Vector3 spawnPosition;

    [Tooltip("The spawn rotation of the character.")]
    public Vector3 spawnRotation;

    //In case we change the character prefab for different scenes for same various reasons.
    [Tooltip("The character to spawn.")]
    public GameObject characterToSpawn;
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

#pragma warning disable 0649

    /// <summary>
    /// Should be constructed/edited within the Editor, not in runtime.
    /// </summary>
    [SerializeField]
    private SpawnData[] spawnData;

#pragma warning restore 0649

    public delegate void CharacterSpawnHandler(GameObject spawnedObject);
    public event CharacterSpawnHandler CharacterSpawned;

    #region Initialization

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoad;

    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoad;

    #endregion Initialization

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        foreach (var data in spawnData)
        {
            if (scene.name == data.sceneName)
                SpawnCharacter(data.characterToSpawn, data.spawnPosition, data.spawnRotation);
        }
    }

    /// <summary>
    /// Spawns the character in scene.
    /// </summary>
    /// <param name="prefabToSpawn">The character's prefab to spawn.</param>
    /// <param name="spawnPos">Spawn position of the character.</param>
    /// <param name="spawnRot">Spawn rotation of the character.</param>
    public void SpawnCharacter(GameObject prefabToSpawn, Vector3 spawnPos, Vector3 spawnRot)
    {
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPos, Quaternion.Euler(spawnRot));
        CharacterSpawned?.Invoke(spawnedObject);
    }

    /// <summary>
    /// Spawns the character in scene.
    /// </summary>
    /// <param name="prefabToSpawn">The character's prefab/GameObject to spawn.</param>
    /// <param name="spawnPos">Spawn position of the character.</param>
    public void SpawnCharacter(GameObject prefabToSpawn, Vector3 spawnPos)
    {
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
        CharacterSpawned?.Invoke(spawnedObject);
    }
}