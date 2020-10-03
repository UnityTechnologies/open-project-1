using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    //TODO - Use an enum to save the scenes

    public void LoadScene(string scene)
    {
        //TODO - Loading Animation
        SceneManager.LoadScene(scene);
    }
}
