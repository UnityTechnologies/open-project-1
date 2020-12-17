using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

class LoadSceneWindow : EditorWindow 
{
    [SerializeField]
    public static AllScenesHolderSO scenesData;

    public SceneButtons[] sceneButtons = null;

    Vector2 scrollView;

    const string NO_SCENES_MESSAGE = "There are no Scenes Assigned in the provided Scene Data";
    const string NOT_ASSINGED_MESSAGE = "There is no Scene Data Asssigned";

    [MenuItem("ChopChop/Scene Management/Scene Loader Window")]
    public static void ShowWindow() 
    {
        EditorWindow.GetWindow(typeof(LoadSceneWindow), false, "Load A Scene", true);
    }

    void OnEnable()
    {
        LoadSceneData(scenesData);
    }

    void OnGUI() 
    {
        GUILayout.Space(20);

        EditorGUI.BeginChangeCheck();
        scenesData = (AllScenesHolderSO) EditorGUILayout.ObjectField("Scenes Index", scenesData, typeof(AllScenesHolderSO), false);
        if (EditorGUI.EndChangeCheck())
        {
            LoadSceneData(scenesData);
        }
        
        GUILayout.Space(15);
        GUILayout.Label("A small Scene Loader Window.", EditorStyles.whiteLargeLabel);

        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        LoadSceneData(scenesData);

        EditorGUILayout.EndScrollView();
    }

    public void LoadSceneData(AllScenesHolderSO sceneData)
    {
        if (scenesData == null)
        {
            EditorGUILayout.HelpBox(NOT_ASSINGED_MESSAGE, MessageType.Info);
            return;
        }

        if (sceneData.Scenes.Length == 0 )
        {
            EditorGUILayout.HelpBox(NO_SCENES_MESSAGE, MessageType.Info);
            return;
        }

        sceneButtons = new SceneButtons[scenesData.Scenes.Length];
        for (int i = 0; i < sceneButtons.Length; i++)
        {
            sceneButtons[i] = new SceneButtons();
            sceneButtons[i].sceneName = scenesData.Scenes[i].scene.name;
            sceneButtons[i].scenePath = scenesData.Scenes[i].scenePath;
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
    }
}