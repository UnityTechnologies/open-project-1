#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using AV.UnityEditor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(SimpleAudioEvent))]
public class SimpleAudioEventEditor : Editor
{
    private static GUIContent audioClipContent;
    private static GUIContent volumeContent = new GUIContent("Volume");
    private static GUIContent pitchContent = new GUIContent("Pitch");
    private static GUIStyle labelCentered;
    
    private ReorderableList clipsList;
    private SerializedProperty clips;
    private SerializedProperty volumeRange;
    private SerializedProperty pitchRange;

    private AudioSource audioPreview;
    
    private void OnEnable()
    {
        // Thanks @superpig for that goodie!
        audioPreview = EditorUtility.CreateGameObjectWithHideFlags(
                "Simple Audio Preview", HideFlags.HideAndDontSave).AddComponent<AudioSource>();

        clips = serializedObject.FindProperty("clips");
        volumeRange = serializedObject.FindProperty("volumeRange");
        pitchRange = serializedObject.FindProperty("pitchRange");

        CreateClipsGUI();
    }

    private void OnDisable()
    {
        DestroyImmediate(audioPreview.gameObject);
    }

    private void CreateClipsGUI()
    {
        clipsList = new ReorderableList(serializedObject, clips);
        clipsList.drawHeaderCallback += rect =>
        {
            GUI.Label(rect, audioClipContent);
        };
        clipsList.drawElementCallback += (rect, index, active, focused) =>
        {
            var property = clips.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        };
    }

    public override void OnInspectorGUI()
    {
        if (labelCentered == null)
        {
            labelCentered = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter
            };
            audioClipContent = new GUIContent("Audio Clips", EditorGUIUtility.IconContent("AudioClip Icon").image);
        }
        
        serializedObject.Update();

        clipsList.DoLayoutList();
        EditorGUILayout.Space();
        
        GUILayout.Label(volumeContent, labelCentered);
        EditorGUILayout.PropertyField(volumeRange, GUIContent.none);
        
        GUILayout.Label(pitchContent, labelCentered);
        EditorGUILayout.PropertyField(pitchRange, GUIContent.none);
        EditorGUILayout.Space();
        
        if(GUILayout.Button("Preview"))
            ((AudioEvent)target).Play(audioPreview);
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif