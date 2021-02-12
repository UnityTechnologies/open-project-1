using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="SaveManager"/>: This Class takes care of Saving and Loading the game progress.
/// </summary>
public class SaveManager : MonoBehaviour
{
    [Header("Save Game Event")]
    [SerializeField] private GameObjectEventChannelSO saveChannel;

    [Header("Load Game Event")]
    [SerializeField] private VoidEventChannelSO loadChannel;

    [Header("All Locations In Game")]
    [SerializeField] private LocationSO[] locations;

    [Header("Load Scene Event")]
    [SerializeField] private LoadEventChannelSO loadSceneChannel;

    [Header("Listening On")]
    [SerializeField] private TransformEventChannelSO playerInstantiatedChannel;

    [Header("What to save")]
    [SerializeField] private LocationAnchor playerLocation;
    [SerializeField] private Inventory playerInventory;

    private GameSceneSO[] locationsToLoad;
    private BinaryFormatter binaryFormatter;
    private string savePath;
    private bool sceneLoadedFromHere = false;
    private Vector3 position;

    private void OnEnable()
    {
        if (saveChannel != null)
            saveChannel.OnEventRaised += SaveGame;

        if (loadChannel != null)
            loadChannel.OnEventRaised += LoadGame;

        if(playerInstantiatedChannel != null)
            playerInstantiatedChannel.OnEventRaised += SetPlayerPosition;
    }

    private void OnDisable()
    {
        if (saveChannel != null)
            saveChannel.OnEventRaised -= SaveGame;

        if (loadChannel != null)
            loadChannel.OnEventRaised -= LoadGame;
        
        if(playerInstantiatedChannel != null)
            playerInstantiatedChannel.OnEventRaised -= SetPlayerPosition;
    }

    private void Start()
    {
        binaryFormatter = new BinaryFormatter();
        savePath = "./Saves/chop.chop";
        position = Vector3.zero;
    }

    private void SaveGame(GameObject savePointGO)
    {
        Debug.Log("Saving Game", gameObject);

        SavePoint savePoint = savePointGO.GetComponent<SavePoint>();
        if (savePoint == null)
        {
            Debug.Log("Invalid Saving Event Raised. Not triggered by a SavePoint",gameObject);
            return;
        }

        PlayerSaveData saveData = new PlayerSaveData(playerLocation, savePoint.playerSpawnPoint, playerInventory);

        FileStream stream = new FileStream(savePath, FileMode.Create);
        var jsonData = JsonUtility.ToJson(saveData);
        binaryFormatter.Serialize(stream, jsonData);

        stream.Close();
    }

    private void LoadGame()
    {
        Debug.Log("Loading Game", gameObject);
        if (File.Exists(savePath))
        {
            FileStream stream = new FileStream(savePath, FileMode.Open);
            var jsonData = binaryFormatter.Deserialize(stream);
            stream.Close();

            PlayerSaveData saveData = new PlayerSaveData();

            JsonUtility.FromJsonOverwrite((string) jsonData, saveData);

            for (int i = 0; i < locations.Length; i++)
            {
                if (locations[i].scenePath.Equals(saveData.playerLocationScenePath))
                {
                    locationsToLoad = new GameSceneSO[1];
                    locationsToLoad[0] = (GameSceneSO) locations[i];
                }
            }
            // player tranform
            position.x = saveData.playerTransformPosition[0];
            position.y = saveData.playerTransformPosition[1];
            position.z = saveData.playerTransformPosition[2];

            // Location
            loadSceneChannel.RaiseEvent(locationsToLoad, false);
            sceneLoadedFromHere = true;
            
            //Inventory
            playerInventory.Items.Clear();
            Debug.Log(saveData.playerItemStack.Count);
            for (int i = 0; i < saveData.playerItemStack.Count; i++)
            {
                playerInventory.Add(saveData.playerItemStack[i].Item, saveData.playerItemStack[i].Amount);
                Debug.Log(saveData.playerItemStack[i].Item);
            }
        }


        else
        {
            Debug.Log("File Not Exist");
            return;
        }
    }
    private void SetPlayerPosition(Transform player)
    {
        if (!sceneLoadedFromHere)
            return;
        
        Debug.Log("Scene Loaded From here (SaveManager), changing player location to the last save point", gameObject);
        player.position = this.position;

        sceneLoadedFromHere = false;
        this.position = Vector3.zero;
    }

    [System.Serializable]
    public class PlayerSaveData
    {
        public string playerLocationScenePath;
        public float[] playerTransformPosition;
        public List<ItemStack> playerItemStack;

        // TODO: QuestSaving also after QuestSystem is completed;

        public PlayerSaveData (LocationAnchor location, Transform transform, Inventory inventory)
        {
            // Location
            playerLocationScenePath = location.Location.scenePath;

            // Save Point
            playerTransformPosition = new float[3];
            playerTransformPosition[0] = transform.position.x;
            playerTransformPosition[1] = transform.position.y;
            playerTransformPosition[2] = transform.position.z;

            // Inventory
            playerItemStack = inventory.Items;

        }

        public PlayerSaveData()
        {
            // Empty Constructor;
        }
    }
}
