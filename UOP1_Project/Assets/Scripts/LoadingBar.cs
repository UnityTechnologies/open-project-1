using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    public GameObject loadingInterface;
    public Image loadingProgressBar;
    public ScenesData scenesData;

    //List of the scenes to load 
    private List<AsyncOperation> currentScenesToLoad = new List<AsyncOperation>();

    public void UpdateProgress()
    {
        StartCoroutine(LoadingScreen());
    }


    IEnumerator LoadingScreen()
    {

        for (int i = 0; i< scenesData.scenesToLoad.Count; ++i)
        {
            currentScenesToLoad.Add(scenesData.scenesToLoad[i]);
        }
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
            for (int i = 0; i < currentScenesToLoad.Count; ++i)
            {
                Debug.Log("check loading of scene " + i + " :" + currentScenesToLoad[i].isDone + "progress scene" + currentScenesToLoad[i].progress);
                //Adding the scene progress to the total progress
                newProgress += currentScenesToLoad[i].progress;
                //the fillAmount for all scenes, so we devide the progress by the number of scenes to load
                loadingProgressBar.fillAmount = totalProgress / currentScenesToLoad.Count;
                Debug.Log("progress bar" + loadingProgressBar.fillAmount + "and value =" + totalProgress / currentScenesToLoad.Count);
            }
            yield return null;
        }
        //Hide progress bar when loading is done
        loadingInterface.SetActive(false);
        currentScenesToLoad.Clear();
    }
}
