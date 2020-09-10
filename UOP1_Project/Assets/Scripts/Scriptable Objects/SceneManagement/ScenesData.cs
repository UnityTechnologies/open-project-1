using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "sceneDB", menuName = "Scene Data/Database")]
public class ScenesData : ScriptableObject
{
    public List<Level> levels = new List<Level>();
    public List<Menu> menus = new List<Menu>();
    public int CurrentLevelIndex=-1;

    //List of the scenes to load and track progress
    public List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    //List of the scenes names to load and unload
    //Scenes to load and unload should be added/Removed to/from the list before calling load and unload methods
    public List<string> scenesToLoadNames = new List<string>();
    public List<string> scenesToUnLoadNames = new List<string>();

    /*
     * Clear
     */

    //Clear Scenes to load and unload list

    public void ClearScenesLists()
    {
        scenesToLoadNames.Clear();
        scenesToUnLoadNames.Clear();
        //Reset index
        CurrentLevelIndex = -1;
    }

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
        scenesToLoadNames.Add(levels[index].sceneName);

        LoadScenes();

        //Add previous level to scenes to unload 
        if (index > 0)
        {
            scenesToUnLoadNames.Add(levels[index - 1].sceneName);
        }

        //Unload previous level if it exists
        if(scenesToUnLoadNames.Count == 0)
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
            if (!CheckLoadState(scenesToLoadNames[i]))
            {
                //Add the scene to the list of scenes to load asynchronously in the background
                scenesToLoad.Add(SceneManager.LoadSceneAsync(scenesToLoadNames[i], LoadSceneMode.Additive));
                //Remove scene from list of scenes to load
                scenesToLoadNames.Remove(scenesToLoadNames[i]);
            }
        }
    }

    public void UnloadScenes()
    {
        for (int i = 0; i < scenesToUnLoadNames.Count; ++i)
        {
            //Unload the scene asynchronously in the background
            SceneManager.UnloadSceneAsync(scenesToUnLoadNames[i]);
            //Remove scene from list of scenes to unload
            scenesToUnLoadNames.Remove(scenesToUnLoadNames[i]);
        }
    }

    //Check if a scene is already loaded
    public bool CheckLoadState(String sceneName)
    {
        if (SceneManager.sceneCount > 0)
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName)
                {
                    return true;
                }
            }
        }
        return false;
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
        //scenesToLoadNames.Add("MainMenu");
        scenesToLoadNames.Add(menus[(int)Type.Main_Menu].sceneName);
        LoadScenes();
    }
    //Load Pause Menu
    public void LoadPauseMenu()
    {
        //Add later if needed
    }
}
