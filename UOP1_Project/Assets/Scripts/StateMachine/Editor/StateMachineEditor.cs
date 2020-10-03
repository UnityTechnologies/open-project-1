#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AV.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace AV.Logic
{
    [CustomEditor(typeof(StateMachine))]
    [CanEditMultipleObjects]
    internal class StateMachineEditor : Editor
    {
        private SerializedProperty stateProperty;

        private void OnEnable()
        {
            stateProperty = serializedObject.FindProperty("currentState");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(stateProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif