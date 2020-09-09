using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    //Check the bool to load MM from ScenesLoaders on start
    public bool LoadOnStart;

    public GameEvent onGameStart;

    public void Start()
    {
        if (LoadOnStart)
        {
            onGameStart.Raise();
        }
    }

    public void Update()
    {
        //We can load the main menu by pressing l in the Scenes Loader scene
        //Just for test purpose
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            onGameStart.Raise();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit dude!");
    }

}
