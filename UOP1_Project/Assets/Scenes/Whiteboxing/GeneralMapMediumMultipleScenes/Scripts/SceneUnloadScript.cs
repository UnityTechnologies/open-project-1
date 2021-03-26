using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloadScript : MonoBehaviour
{
    public string sceneName; //name of scene to unload without path e.g. "myScene"

    private void OnTriggerExit(Collider other)
    {
         for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Debug.Log(SceneManager.GetSceneAt(i).name);
            if (sceneName.Equals(SceneManager.GetSceneAt(i).name))
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }
}
