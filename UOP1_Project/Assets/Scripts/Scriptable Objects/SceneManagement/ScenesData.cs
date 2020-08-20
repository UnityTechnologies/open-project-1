using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "sceneDB", menuName = "Scene Data/Database")]
public class ScenesData : ScriptableObject
{
    public List<Level> levels = new List<Level>();
    public List<Menu> menus = new List<Menu>();
    public int CurrentLevelIndex=0;

    //List of the scenes to load and track progress
    public List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();


    //List of the scenes name to load and unload
    //Scenes to load and unload should be added/Removed to/from the list before calling load and unload methods
    public List<string> scenesToLoadNames = new List<string>();
    public List<string> scenesToUnLoadNames = new List<string>();

    /*
     * Levels
     */

    //Load a scene with a given index
    public void LoadLevelWithIndex(int index)
    {
     
        //reset the level index if we have no more levels
        if (index >= levels.Count)
        {
            index = 1;
            CurrentLevelIndex = index;
        }

        //Load level
        scenesToLoadNames.Add("Level" + index.ToString());
        LoadScenes();

        //Add previous level to scenes to unload 
        if (index > 0)
        {
            scenesToUnLoadNames.Add(levels[index - 1].ToString());
        }

        //Unload level (This needs to be here, in case we need to unload MM)
        if(scenesToUnLoadNames == null)
        {
            return;
        }
        else
        {
            UnloadScenes();
        }
        
    }

    public void LoadScenes()
    {
        for (int i = 0; i < scenesToLoadNames.Count; ++i)
        {
            //Add the scene to the list of scenes to load asynchronously in the background
            scenesToLoad.Add(SceneManager.LoadSceneAsync(scenesToLoadNames[i], LoadSceneMode.Additive));
            //Remove scene from list of scenes to load
            scenesToLoadNames.Remove(scenesToUnLoadNames[i]);
        }
    }

    public void UnloadScenes()
    {
        for (int i = 0; i < scenesToLoad.Count; ++i)
        {
            //Add the scene to the list of scenes to unload asynchronously in the background
            SceneManager.UnloadSceneAsync(scenesToUnLoadNames[i]);
            //Remove scene from list of scenes to unload
            scenesToUnLoadNames.Remove(scenesToUnLoadNames[i]);
        }
    }

    //Start next level
    public void NextLevel()
    {
        CurrentLevelIndex++;
        LoadLevelWithIndex(CurrentLevelIndex);
    }
    //Restart current level
    public void RestartLevel()
    {
        LoadLevelWithIndex(CurrentLevelIndex);
    }
   
   
    /*
     * Menus
     */

    //Load main Menu
    public void LoadMainMenu()
    {
        scenesToLoadNames.Add("MainMenu");
        LoadScenes();
        //Add it to the scenes to unload when the level starts
        scenesToUnLoadNames.Add("MainMenu");
    }
    //Load Pause Menu
    public void LoadPauseMenu()
    {
        //Add later if needed
    }
}
