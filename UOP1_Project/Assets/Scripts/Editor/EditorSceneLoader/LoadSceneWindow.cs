using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

class LoadSceneWindow : EditorWindow 
{
    [SerializeField]
    public AllScenesHolderSO scenesData;

    public SceneButtons[] sceneButtons = null;

    Vector2 scrollView;

    [MenuItem("ChopChop/Scene Management/Scene Loader Window")]
    public static void ShowWindow() 
    {
        EditorWindow.GetWindow(typeof(LoadSceneWindow), false, "Load A Scene", true);
    }

    void OnGUI() 
    {
        GUILayout.Space(30);
        scenesData = (AllScenesHolderSO) EditorGUILayout.ObjectField("Scenes Index", scenesData, typeof(AllScenesHolderSO), false);
        GUILayout.Space(15);
        GUILayout.Label("A small Scene Loader Window.", EditorStyles.boldLabel);

        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        if (scenesData == null)
            return;
        
        if (sceneButtons == null)
        {
            sceneButtons = new SceneButtons[scenesData.Scenes.Length];
            for (int i = 0; i < sceneButtons.Length; i++)
            {
                sceneButtons[i] = new SceneButtons();
                sceneButtons[i].sceneName = scenesData.Scenes[i].sceneDisplayName;
                sceneButtons[i].scenePath = scenesData.Scenes[i].scenePath;
            }
        }

        for (int i = 0; i < sceneButtons.Length; i++)
        {
            if (!EditorSceneManager.GetActiveScene().name.Equals(sceneButtons[i].sceneName))
            {
                if (sceneButtons[i].button = GUILayout.Button(sceneButtons[i].sceneName, GUILayout.Width(250), GUILayout.Height(35)))
                {
                    sceneButtons[i].PressButton();
                }
                GUILayout.Space(20);
            }
        }

        EditorGUILayout.EndScrollView();
    }
}

class SceneButtons
{
    public bool button;
    public string sceneName;
    public string scenePath;

    public void PressButton()
    {
        EditorSceneManager.OpenScene(scenePath);
        //Debug.Log(scenePath);
    }
}