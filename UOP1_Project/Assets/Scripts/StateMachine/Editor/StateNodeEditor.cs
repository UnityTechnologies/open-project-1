using System;
using AV.UnityEditor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AV.Logic
{
    [CustomEditor(typeof(StateNode))]
    internal class StateNodeEditor : Editor
    {
        private class Content
        {
            public static GUIContent systemsContent;
            public static GUIContent transitionsContent;
            public static GUIContent plusMore;
            public static GUIContent minus;
        }

        private const int ElementsSpacing = 4;
        
        private static GUIStyle preButton;
        private static GUIStyle footerBackground;
        private static GenericMenu transitionAddMenu;
        
        private SerializedProperty actionsProperty;
        private SerializedProperty transitionsProperty;

        private ReorderableList actionsList;
        private ReorderableList transitionsList;
        private ReorderableList[] decisionsLists;
        private ReorderableList[] transitionActionLists;
        private int lastTransitionsCount;
        private ReorderableList selectedList;
        private SerializedProperty transitionAddTarget;

        private void OnEnable()
        {
            Content.systemsContent = new GUIContent("Actions", EditorGUIUtility.IconContent("AnimatorState Icon").image);
            Content.transitionsContent = new GUIContent("Transitions",
                EditorGUIUtility.IconContent("AnimatorStateTransition Icon").image);
            
            Content.plusMore = EditorGUIUtility.IconContent("Toolbar Plus More");
            Content.plusMore.tooltip = "Add node";
            Content.minus = EditorGUIUtility.IconContent("Toolbar Minus");
            Content.minus.tooltip = "Remove selected element";
            
            actionsProperty = serializedObject.FindProperty("actions");
            transitionsProperty = serializedObject.FindProperty("transitions");
            
            CreateActionsGUI();
            CreateTransitionsGUI();
            CreateTransitionAddMenu();
        }
        
        private static void GetGUIStyles()
        {
            if (preButton != null) return;
            preButton = "RL FooterButton";
            footerBackground = "RL Footer";
        }
        
        public override void OnInspectorGUI()
        {
            GetGUIStyles();

            serializedObject.Update();

            if (lastTransitionsCount != transitionsProperty.arraySize)
            {
                lastTransitionsCount = transitionsProperty.arraySize;
                CreateTransitionsGUI();
            }

            actionsList.DoLayoutList();
            EditorGUILayout.Space();
            transitionsList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void CreateTransitionAddMenu()
        {
            transitionAddMenu = new GenericMenu();
            
            transitionAddMenu.AddItem(new GUIContent("Decision"), false, () =>
            {
                var decisions = transitionAddTarget.FindPropertyRelative("decisions");
                
                decisions.InsertArrayElementAtIndex(0);
                decisions.GetArrayElementAtIndex(0).objectReferenceValue = null;
                serializedObject.ApplyModifiedProperties();
            });
            
            transitionAddMenu.AddItem(new GUIContent("Action"), false, () =>
            {
                var actions = transitionAddTarget.FindPropertyRelative("actions");
                
                actions.InsertArrayElementAtIndex(0);
                actions.GetArrayElementAtIndex(0).FindPropertyRelative("state").objectReferenceValue = null;
                serializedObject.ApplyModifiedProperties();
            });        
        }

        private void CreateActionsGUI()
        {
            actionsList = new ReorderableList(serializedObject, actionsProperty)
            {
                elementHeight = EditorGUIUtility.singleLineHeight + ElementsSpacing
            };
            
            actionsList.drawHeaderCallback += rect =>
            {
                GUI.Label(rect, Content.systemsContent);
            };
            
            actionsList.drawElementCallback += (elementRect, index, active, focused) =>
            {
                var property = actionsProperty.GetArrayElementAtIndex(index);
                var trigger = property.FindPropertyRelative("trigger");
                var logic = property.FindPropertyRelative("logic");
                
                var rect = new Rect(elementRect);
                rect.height -= ElementsSpacing;

                rect.width = 90;
                EditorGUI.PropertyField(rect, trigger, GUIContent.none);
                rect.x += rect.width;
                
                rect.width = elementRect.width - 90;
                    
                EditorGUI.PropertyField(rect, logic, GUIContent.none);
                rect.y += ElementsSpacing;
            };
        }

        private void CreateTransitionsGUI()
        {
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var lineSpacing = lineHeight + 4;

            transitionsList = new ReorderableList(serializedObject, transitionsProperty);
            
            transitionsList.drawHeaderCallback += rect =>
            {
                GUI.Label(rect, Content.transitionsContent);
            };
            
            transitionsList.elementHeightCallback += index =>
            {
                var property = transitionsProperty.GetArrayElementAtIndex(index);
                var decisions = property.FindPropertyRelative("decisions");
                var actions = property.FindPropertyRelative("actions");
                
                var height = lineSpacing;
                
                height += lineSpacing * Mathf.Max(decisions.arraySize, 1);
                height += lineSpacing * Mathf.Max(actions.arraySize, 1);
                
                return height;
            };

            transitionsList.drawElementCallback += (elementRect, index, active, focused) =>
            {
                var rect = new Rect(elementRect);

                rect.y += ElementsSpacing;
                rect.height = lineHeight;
                
                var transition = transitionsProperty.GetArrayElementAtIndex(index);

                var enabled = transition.FindPropertyRelative("enabled");
                var decisions = transition.FindPropertyRelative("decisions");
                var actions = transition.FindPropertyRelative("actions");
                
                var decisionsList = decisionsLists[index];
                var actionsList = transitionActionLists[index];
                
                var isSomethingSelected = selectedList != null && selectedList.index != -1;
                var isDecisionSelected = isSomethingSelected && selectedList == decisionsList;
                var isActionSelected = isSomethingSelected && selectedList == actionsList;
                
                
                rect.width = 20;
                EditorGUI.PropertyField(rect, enabled, GUIContent.none);
                rect.width = elementRect.width;
                
                EditorGUI.BeginDisabledGroup(!enabled.boolValue);

                rect.x += 20;
                rect.width -= 20;
                
                decisionsList.DoList(rect);
                rect.y += lineSpacing * Mathf.Max(decisions.arraySize, 1) + ElementsSpacing + 1;

                var addRemoveRect = new Rect(rect) { width = 20, height = 20 };
                
                addRemoveRect.x += 5;
                addRemoveRect.y += 5;
                if (GUI.Button(addRemoveRect, Content.plusMore, preButton))
                {
                    transitionAddTarget = transition;
                    transitionAddMenu.DropDown(addRemoveRect);
                }
                
                EditorGUI.BeginDisabledGroup(!isDecisionSelected && !isActionSelected);
                
                addRemoveRect.x += 20;
                if (GUI.Button(addRemoveRect, Content.minus, preButton))
                {
                    if(isDecisionSelected)
                        decisions.DeleteArrayElementAtIndex(selectedList.index);
                    else if(isActionSelected)
                        actions.DeleteArrayElementAtIndex(selectedList.index);
                }
                EditorGUI.EndDisabledGroup();
                
                rect.x += 50;
                rect.width -= 50;
                actionsList.DoList(rect);
                
                EditorGUI.EndDisabledGroup();
            };
            
            CreateDecisionsGUI();
            CreateEventsGUI();
        }

        private void CreateDecisionsGUI()
        {
            decisionsLists = new ReorderableList[transitionsProperty.arraySize];
            
            for (int i = 0; i < transitionsProperty.arraySize; i++)
            {
                var list = decisionsLists[i];
                var transition = transitionsProperty.GetArrayElementAtIndex(i);
                var decisions = transition.FindPropertyRelative("decisions");

                list = new ReorderableList(serializedObject, decisions, true, false, false, false)
                {
                    elementHeight = EditorGUIUtility.singleLineHeight + ElementsSpacing,
                    headerHeight = 1
                };
                
                list.onSelectCallback += reorderableList =>
                {
                    selectedList = reorderableList;
                };
                
                list.drawElementCallback += (rect, index, active, focused) =>
                {
                    var element = decisions.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, element, GUIContent.none);
                };
                decisionsLists[i] = list;
            }
        }

        private void CreateEventsGUI()
        {
            transitionActionLists = new ReorderableList[transitionsProperty.arraySize];
            
            for (int i = 0; i < transitionsProperty.arraySize; i++)
            {
                var list = transitionActionLists[i];
                var transition = transitionsProperty.GetArrayElementAtIndex(i);
                var actions = transition.FindPropertyRelative("actions");

                list = new ReorderableList(serializedObject, actions, true, false, false, false)
                {
                    headerHeight = 0
                };
                
                list.onSelectCallback += reorderableList =>
                {
                    selectedList = reorderableList;
                };
                
                list.drawElementCallback += (rect, index, active, focused) =>
                {
                    var element = actions.GetArrayElementAtIndex(index);
                    var trigger = element.FindPropertyRelative("trigger");
                    var state = element.FindPropertyRelative("state");
                    var type = element.FindPropertyRelative("type");

                    rect.y += ElementsSpacing / 2f;
                    rect.height -= ElementsSpacing;
                    
                    var triggerRect = new Rect(rect) { width = 70 };
                    EditorGUI.PropertyField(triggerRect, trigger, GUIContent.none);
                    
                    rect.x += triggerRect.width;
                    rect.width -= triggerRect.width;
                    EditorGUI.PropertyField(rect, state, GUIContent.none);

                    var reference = state.objectReferenceValue;
                    if (reference != null)
                    {
                        switch (reference)
                        {
                            case StateNode _: type.enumValueIndex = (int) StateTransition.StateType.Node; break;
                            case StateAction _: type.enumValueIndex = (int) StateTransition.StateType.Action; break;
                            case StateDecision _: type.enumValueIndex = (int) StateTransition.StateType.Decision; break;
                        }
                    }
                };
                transitionActionLists[i] = list;
            }
        }
    }
}