using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadScript : MonoBehaviour
{
    public string scenePath; //path of scene to load inside "Assets" without scenename e.g. here "SeemlessSceneLoadDemo/Scenes/"
    public string sceneName; //scenename e.g. "myScene"
    private void OnTriggerEnter(Collider other)
    {
        bool isSceneLoaded = false;
        //first check if scene is already loaded
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (sceneName.Equals(SceneManager.GetSceneAt(i).name))
            {
                isSceneLoaded = true;
            }
        }
        if (!isSceneLoaded)
        {
            SceneManager.LoadScene(scenePath+ sceneName, LoadSceneMode.Additive);
        }
    }
}
