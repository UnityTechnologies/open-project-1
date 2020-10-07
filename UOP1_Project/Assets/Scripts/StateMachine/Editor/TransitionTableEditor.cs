using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KarimCastagnini.PluggableFSM
{
    /*
     * UX NOTE:
     * The Asset picker doesn't work because types are of Generics.
     * For example we have DataModelAction : Action<DataModel>.
     * In the inspector we are asking for a Action<DataModel> field, and because of that
     * we can drag a DataModelAction into the field. But if we try to use the asset picker, it will found nothing
     * because it's searching for a Action<DataModel> but can't find it because assets are only DataModelAction.
     * A cool thing would be to display all current asset that are subtypes of Action<DataModel>, but there's no
     * such a feature for the asset picker right now.
     * UX "fix": get a square with "Drag [class name] here to add" and avoid using the asset picker ):
     */
    [CustomEditor(typeof(TransitionTableBase), true)]
    public class TransitionTableEditor : Editor
    {
        private enum ConditionResult
        {
            True = 0,
            False
        }

        private TransitionTableBase _target;
        private Type _stateType;
        private Type _conditionType;
        private SerializedProperty _initialState;
        private ReorderableList _stateToStateList;
        private ReorderableList _anyStateToStateList;

        private void OnEnable()
        {          
            _target = (TransitionTableBase)serializedObject.targetObject;
           
            //data types to expose in the inspector fields, there's probably a more elegant way
            _stateType = _target.GetStateType();
            _conditionType = _target.GetConditionType();

            _initialState = serializedObject.FindProperty("_initialState");

            _stateToStateList = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("_stateToStateEntries"),
                true, true, true, true);

            _stateToStateList.drawElementCallback = DrawStateToStateEnty;
            _stateToStateList.drawHeaderCallback = (rect) => GUI.Label(rect, "State to state transitions:");

            _anyStateToStateList = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("_anyStateToStateEntries"),
                true, true, true, true);

            _anyStateToStateList.drawElementCallback = DrawAnyStateToStateEnty;
            _anyStateToStateList.drawHeaderCallback = (rect) => GUI.Label(rect, "Any state to state transitions:");
        }

        public override void OnInspectorGUI()
        {
            bool playing = EditorApplication.isPlayingOrWillChangePlaymode || Application.isPlaying;

            EditorGUI.BeginDisabledGroup(playing);
            serializedObject.Update(); //get the latest target version
            DrawInspector();
            serializedObject.ApplyModifiedProperties(); //save changes
            //serializedObject.ApplyModifiedPropertiesWithoutUndo(); //add support for undo
            EditorGUI.EndDisabledGroup();

            if (playing)
                EditorGUILayout.HelpBox("Editing during PlayMode currently not supported.", MessageType.Info);
        }

        private void DrawInspector()
        {
            _initialState.objectReferenceValue = 
                EditorGUILayout.ObjectField("Initial State: ", _initialState.objectReferenceValue, _stateType, false) as Object;

            EditorGUILayout.Space(10);     
            _stateToStateList.DoLayoutList();
            EditorGUILayout.Space(10);     
            _anyStateToStateList.DoLayoutList();
        }

        private void DrawStateToStateEnty(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = _stateToStateList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            SerializedProperty fromState = element.FindPropertyRelative("FromState");
            SerializedProperty toState = element.FindPropertyRelative("ToState");
            SerializedProperty condition = element.FindPropertyRelative("Condition");
            SerializedProperty conditionResult = element.FindPropertyRelative("ConditionResult");
                       
            float fieldWidth = rect.width / 4f;

            Rect fromRect = new Rect(rect.x, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight);
            Rect toRect = new Rect(rect.x + fieldWidth, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight);
            Rect ifRect = new Rect(rect.x + fieldWidth * 2f, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight);
            Rect isRect = new Rect(rect.x + fieldWidth * 3f, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight);

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = rect.width / 20f;

            fromState.objectReferenceValue = 
                EditorGUI.ObjectField(fromRect, "from", fromState.objectReferenceValue, _stateType, false) as Object;

            toState.objectReferenceValue = 
                EditorGUI.ObjectField(toRect, " to", toState.objectReferenceValue, _stateType, false) as Object;

            condition.objectReferenceValue = 
                EditorGUI.ObjectField(ifRect, " if", condition.objectReferenceValue, _conditionType, false) as Object;

            ConditionResult res = conditionResult.boolValue ? ConditionResult.True : ConditionResult.False;
            res = (ConditionResult)EditorGUI.EnumPopup(isRect, " is", res);
            conditionResult.boolValue = res == ConditionResult.True ? true : false;

            EditorGUIUtility.labelWidth = labelWidth;
        }

        //atrocious copy & paste with slight modifications, will refactor
        private void DrawAnyStateToStateEnty(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = _anyStateToStateList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            SerializedProperty toState = element.FindPropertyRelative("ToState");
            SerializedProperty condition = element.FindPropertyRelative("Condition");
            SerializedProperty conditionResult = element.FindPropertyRelative("ConditionResult");

            float fieldWidth = rect.width / 3f;

            Rect toRect = new Rect(rect.x, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight);
            Rect ifRect = new Rect(rect.x + fieldWidth, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight);
            Rect isRect = new Rect(rect.x + fieldWidth * 2f, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight);

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = rect.width / 20f;

            toState.objectReferenceValue =
                EditorGUI.ObjectField(toRect, " to", toState.objectReferenceValue, _stateType, false) as Object;

            condition.objectReferenceValue =
                EditorGUI.ObjectField(ifRect, " if", condition.objectReferenceValue, _conditionType, false) as Object;

            ConditionResult res = conditionResult.boolValue ? ConditionResult.True : ConditionResult.False;
            res = (ConditionResult)EditorGUI.EnumPopup(isRect, " is", res);
            conditionResult.boolValue = res == ConditionResult.True ? true : false;

            EditorGUIUtility.labelWidth = labelWidth;
        }
    }
}