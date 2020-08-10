using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingInterface;
    public Image loadingProgressBar;
    //List of the scenes to load 
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    //List of the scenes name to load and unload
    //Scenes to load and unload should be added/Removed to/from the list before calling load and unload methods
    List<string> scenesToLoadNames = new List<string>();
    List<string> scenesToUnLoadNames = new List<string>();

    void Start()
    {
        StartMainMenu();
    }

    public void StartMainMenu()
    {
        scenesToLoadNames.Add("MainMenu");
        LoadScenes(false);
    }

    public void StartLevel()
    {
        ShowLoadingScreen(true);
        scenesToLoadNames.Add("Level1");
        LoadScenes(true);
        scenesToUnLoadNames.Add("MainMenu");
        UnloadScenes();
    }


    public void LoadScenes(bool EnableLoadingBar)
    {

        for (int i = 0; i < scenesToLoad.Count; ++i)
        {
            //Add the scene to the list of scenes to load asynchronously in the background
            scenesToLoad.Add(SceneManager.LoadSceneAsync(scenesToLoadNames[i], LoadSceneMode.Additive));
            //Remove scene from list of scenes to load
            scenesToLoadNames.Remove(scenesToUnLoadNames[i]);
        }
        //Track the progress for the bar
        if (EnableLoadingBar)
        {
            StartCoroutine(LoadingScreen());
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

    public void ShowLoadingScreen(bool state)
    {
        loadingInterface.SetActive(state);
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress = 0, newProgress = 0;
        //When the scene reaches 0.9f, it means that it is loaded
        //The remaining 0.1f are for the integration
        while (totalProgress <= 0.9f)
        {
            //Get the latest progress value
            totalProgress = newProgress;
            //Reset the progress for the new values
            newProgress = 0;
            //Iterate through all the scenes to load
            for (int i = 0; i < scenesToLoad.Count; ++i)
            {
                Debug.Log("check loading of scene " + i + " :" + scenesToLoad[i].isDone + "progress scene" + scenesToLoad[i].progress);
                //Adding the scene progress to the total progress
                newProgress += scenesToLoad[i].progress;
                //the fillAmount for all scenes, so we devide the progress by the number of scenes to load
                loadingProgressBar.fillAmount = totalProgress / scenesToLoad.Count;
                Debug.Log("progress bar" + loadingProgressBar.fillAmount + "and value =" + totalProgress / scenesToLoad.Count);
            }
            yield return null;
        }
        //Hide progress bar when loading is done
        ShowLoadingScreen(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
