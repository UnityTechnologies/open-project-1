using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "AllScenesReference", menuName = "Scene Data/All Scenes Holder")]
public class AllScenesHolderSO : ScriptableObject
{
    [Header("Scenes List")]
    public SceneData[] Scenes;

    void OnEnable()
    {
        for (int i = 0; i < Scenes.Length; i++)
        {
            if (Scenes[i].sceneDisplayName.Equals(""))
                Scenes[i].sceneDisplayName = Scenes[i].scene.name;
                
            Scenes[i].scenePath = AssetDatabase.GetAssetPath(Scenes[i].scene.GetInstanceID());
        }
        //Debug.Log(Scenes.Length);
    }
}

[System.Serializable]
public struct SceneData
{
    public SceneAsset scene;
    public string sceneDisplayName;
    public string scenePath;
}
