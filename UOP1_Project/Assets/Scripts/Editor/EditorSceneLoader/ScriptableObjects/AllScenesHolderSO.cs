using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;
using UnityEditorInternal;

[System.Serializable]
public struct SceneData
{
    public SceneAsset scene;
    public string scenePath;
}

[CreateAssetMenu(fileName = "Scenes Reference", menuName = "Scene Data/Scenes Reference")]
public class AllScenesHolderSO : ScriptableObject
{
    [Header("Scenes List")]
    public SceneData[] Scenes;

    public void UpdateScenesPath()
    {
        if (Scenes.Length > 0)
        {
            for (int i = 0; i < Scenes.Length; i++)
            {                
                Scenes[i].scenePath = AssetDatabase.GetAssetPath(Scenes[i].scene.GetInstanceID());
            }
        }
    }
}

[CustomEditor(typeof(AllScenesHolderSO))]
public class AllScenesHolderSOEditor : Editor
{
    AllScenesHolderSO scenesHolder;
    ReorderableList scenesList;

    public void OnEnable()
    {
        scenesHolder = (AllScenesHolderSO) target;
        scenesList = new ReorderableList(serializedObject, serializedObject.FindProperty("Scenes"), true, true, true, true);

        scenesList.elementHeight = 80f;

        scenesList.drawElementCallback = 
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = scenesList.serializedProperty.GetArrayElementAtIndex(index);
                
                var scene = element.FindPropertyRelative("scene");
                //var name = element.FindPropertyRelative("sceneDisplayName");
                var path = element.FindPropertyRelative("scenePath");

                rect.height = 20f;
                rect.y += 2;
                
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), scene, GUIContent.none);
                EditorGUI.TextField(new Rect(rect.x, rect.y + 30, rect.width - 5, EditorGUIUtility.singleLineHeight), path.stringValue);

                EditorGUI.DrawRect(new Rect(rect.x - 16, rect.y + 60, rect.width, 1.5f), Color.gray);

            };

        scenesList.onSelectCallback = (ReorderableList list) =>
            {
                scenesHolder.UpdateScenesPath();
            };

        scenesList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Scenes List");
            };
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        if (GUILayout.Button("Update All Paths"))
        {
            scenesHolder.UpdateScenesPath();
        }
        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        serializedObject.Update();
        scenesList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}