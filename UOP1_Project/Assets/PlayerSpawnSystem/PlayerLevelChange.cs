using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//The main "Spawn System" management class. Call the
public class PlayerLevelChange : MonoBehaviour
{
    public static PlayerLevelChange main;
    [SerializeField] List<PlayerSpawnPoint> spawnPoints = new List<PlayerSpawnPoint>(); //Contains all the spawn points in the current scene

    void Awake()
    {
        //========== Singleton design stuff ==========
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }

        //========== Initialization ==========
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearSpawnPoints();
    }

    void ClearSpawnPoints()
    {
        spawnPoints.Clear();
    }

    public void AddSpawnPoint(PlayerSpawnPoint sp)
    {
        spawnPoints.Add(sp);
    }

    //Teleports/'Spawns' the player at the specified spawn point in the current scene
    public void SpawnPlayerAtSpawnPoint(Protagonist player, string spawnPointLabel)
    {
        PlayerSpawnPoint sp = spawnPoints.First(point => point.label == spawnPointLabel);
        Spawn(player, sp.transform.position, sp.transform.rotation);
    }

    void Spawn(Protagonist player, Vector3 pos, Quaternion rot)
    {
        player.characterController.enabled = false; //Disable the controller due to it interfering with changing the position directly
        player.transform.position = pos;
        player.characterController.enabled = true;

        player.transform.rotation = rot;
    }
    
    public string[] GetSpawnPointLabels()
    {
        string[] res = new string[spawnPoints.Count];
        int i = 0;
        foreach(PlayerSpawnPoint sp in spawnPoints)
        {
            res[i] = sp.label;
            i++;
        }
        return res;
    }

    //For testing purposes: Type the spawn point label into the "Spawn Label" field in the inspector, and hit the "Spawn" button in the game window
    //"Labels" prints all spawn points to the console
    [SerializeField] string spawnLabel = "";
    void OnGUI()
    {
        GUI.backgroundColor = Color.white;
        if(GUI.Button(new Rect(10, 10, 50, 30), "Spawn"))
        {
            SpawnPlayerAtSpawnPoint(GameObject.Find("Pig").GetComponent<Protagonist>(),spawnLabel);
        }
        if(GUI.Button(new Rect(10, 70, 50, 30), "Labels"))
        {
            Debug.Log(string.Join("\n" ,GetSpawnPointLabels()));
        }
    }

}
