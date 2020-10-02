using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GameScene), true)]
public class GameSceneEditor : Editor
{
    #region UI_Warnings

    private string noScenesWarning = "There are no scenes set for this level yet! Add a new scene with the dropdown above";
    
    #endregion
    
    // it holds a list of properties to hide from basic inspector
    private static readonly string[] ExcludedProperties = {"m_Script", "sceneName"};
    
    private string[] sceneList;
    private GameScene gameSceneTarget;

    private void OnEnable()
    {
        gameSceneTarget = target as GameScene;
    }

    public override void OnInspectorGUI()
    {
        PopulateScenePicker();
        EditorGUILayout.LabelField("Scene information", new GUIStyle(EditorStyles.largeLabel) {fontStyle = FontStyle.Bold, 
            fontSize = 18, fixedHeight = 70.0f});
        EditorGUILayout.Space();
        DrawScenePicker();
        DrawPropertiesExcluding(serializedObject, ExcludedProperties);
    }

    private void DrawScenePicker()
    {
        var sceneName = gameSceneTarget.sceneName;
        EditorGUI.BeginChangeCheck();
        var selectedScene = sceneList.ToList().IndexOf(sceneName);
        selectedScene = EditorGUILayout.Popup("Scene", selectedScene, sceneList);
        if (selectedScene > -1)
        {
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed selected scene");
                gameSceneTarget.sceneName = sceneList[selectedScene];
                MarkAllDirty();
            }
        }
        else
        {
            EditorGUILayout.HelpBox(noScenesWarning, MessageType.Warning);
        }
    }

    /// <summary>
    /// Populates the scene picker with scenes included in the game's build index
    /// </summary>
    private void PopulateScenePicker()
    {
        var sceneCount = SceneManager.sceneCountInBuildSettings;
        sceneList = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            sceneList[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
    }
    
    /// <summary>
    /// Marks scenes as dirty so data can be saved
    /// </summary>
    private void MarkAllDirty()
    {
        EditorUtility.SetDirty(target);
        EditorSceneManager.MarkAllScenesDirty();
    }
}
