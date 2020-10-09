using System;
using AV.UnityEditor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.EditorGUIUtility;

namespace AV.Logic
{
    [CustomEditor(typeof(StateNode))]
    internal class StateNodeEditor : Editor
    {
        private static class Guids
        {
            public const string ActionIcon = "ac3b011b7b5a64548956402b6c471cef";
            public const string StateChangeIcon = "e9215b03c7583f54a870c9a16a7d3885";
        }
        private static class Icons
        {
            public static GUIContent action;
            public static GUIContent stateChange;
        }
        private static class Content
        {
            public static GUIContent systemsContent;
            public static GUIContent transitionsContent;
            public static GUIContent plusMore;
            public static GUIContent minus;
        }
        private static class Styles
        {
            public static GUIStyle iconLabel;
            public static GUIStyle preButton;
            public static GUIStyle footerBackground;
        }

        private static GenericMenu transitionAddMenu;

        private StateNode target;
        
        private SerializedProperty actionsProperty;
        private SerializedProperty transitionsProperty;

        private ReorderableList actionsList;
        private ReorderableList transitionsList;
        private ReorderableList[] decisionsLists;
        private ReorderableList[] transitionActionLists;
        private int lastTransitionsCount;
        private ReorderableList selectedList;
        private SerializedProperty transitionAddTarget;
        private int lastPlusIndex;

        
        private void OnEnable()
        {
            GetGUIContent();

            target = (StateNode) base.target;
            
            actionsProperty = serializedObject.FindProperty("actions");
            transitionsProperty = serializedObject.FindProperty("transitions");
            
            CreateActionsGUI();
            CreateTransitionsGUI();
        }

        private static void GetGUIContent()
        { 
            var actionIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(Guids.ActionIcon));
            var stateChangeIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(Guids.StateChangeIcon));
            
            Icons.action = new GUIContent(actionIcon, "Invoke State Action");
            Icons.stateChange = new GUIContent(stateChangeIcon, "State Change");
            
            Content.systemsContent = new GUIContent("Actions", EditorGUIUtility.IconContent("AnimatorState Icon").image);
            Content.transitionsContent = new GUIContent("Transitions",
                EditorGUIUtility.IconContent("AnimatorStateTransition Icon").image);
            
            Content.plusMore = EditorGUIUtility.IconContent("Toolbar Plus More");
            Content.plusMore.tooltip = "Add node";
            Content.minus = EditorGUIUtility.IconContent("Toolbar Minus");
            Content.minus.tooltip = "Remove selected element";
        }
        
        private static void GetGUIStyles()
        {
            if (Styles.preButton != null) 
                return;
            Styles.iconLabel = new GUIStyle(EditorStyles.label) { padding = new RectOffset(4, 4, 0, 0) };
            Styles.preButton = "RL FooterButton";
            Styles.footerBackground = "RL Footer";
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
            
            transitionAddMenu.AddSeparator("");
            
            var hasChangeStateAction = false;
            var transition = target.transitions[lastPlusIndex];
            foreach (var action in transition.actions)
            {
                if (action.type == StateTransition.ActionType.ChangeState)
                    hasChangeStateAction = true;
            }
            
            transitionAddMenu.AddItem(new GUIContent("Action"), false, () =>
            {
                var actions = transitionAddTarget.FindPropertyRelative("actions");
                
                actions.InsertArrayElementAtIndex(0);
                actions.GetArrayElementAtIndex(0).FindPropertyRelative("state").objectReferenceValue = null;
                serializedObject.ApplyModifiedProperties();
                
                transition.actions[0].type = StateTransition.ActionType.Action;
            });

            if (!hasChangeStateAction)
            {
                transitionAddMenu.AddItem(new GUIContent("State Change"), false, () =>
                {
                    var actions = transitionAddTarget.FindPropertyRelative("actions");

                    actions.InsertArrayElementAtIndex(0);
                    actions.GetArrayElementAtIndex(0).FindPropertyRelative("state").objectReferenceValue = null;
                    
                    serializedObject.ApplyModifiedProperties();
                    
                    transition.actions[0].type = StateTransition.ActionType.ChangeState;
                });
            }
            else
            {
                transitionAddMenu.AddDisabledItem(new GUIContent("State Change"), true);
            }
        }

        private void CreateActionsGUI()
        {
            actionsList = new ReorderableList(serializedObject, actionsProperty)
            {
                elementHeight = singleLineHeight + standardVerticalSpacing
            };
            
            actionsList.drawHeaderCallback += rect =>
            {
                GUI.Label(rect, Content.systemsContent);
            };
            
            actionsList.drawElementCallback += (rect, index, active, focused) =>
            {
                var property = actionsProperty.GetArrayElementAtIndex(index);
                var trigger = property.FindPropertyRelative("trigger");
                var logic = property.FindPropertyRelative("logic");

                rect.height = singleLineHeight;
                rect.y += standardVerticalSpacing / 2;
                
                var popupRect = new Rect(rect) { width = 90 };

                EditorStyles.popup.fontSize--;
                EditorGUI.PropertyField(popupRect, trigger, GUIContent.none);
                EditorStyles.popup.fontSize++;
                
                rect.x += popupRect.width;
                rect.width -= popupRect.width;
                    
                EditorGUI.PropertyField(rect, logic, GUIContent.none);
            };
        }

        private void CreateTransitionsGUI()
        {
            var lineHeight = singleLineHeight + standardVerticalSpacing;
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

                var height = lineHeight;
                
                height += lineHeight * Mathf.Max(decisions.arraySize, 1);
                height += lineHeight * Mathf.Max(actions.arraySize, 1);
                
                return height;
            };

            transitionsList.drawElementCallback += (elementRect, index, active, focused) =>
            {
                var rect = new Rect(elementRect);

                var verticalSpacing = standardVerticalSpacing;
                rect.y += verticalSpacing;
                rect.height = singleLineHeight;
                
                var transition = transitionsProperty.GetArrayElementAtIndex(index);

                var enabled = transition.FindPropertyRelative("enabled");
                var decisions = transition.FindPropertyRelative("decisions");
                var actions = transition.FindPropertyRelative("actions");
                
                var decisionsList = decisionsLists[index];
                var decisionActionsList = transitionActionLists[index];
                
                var isSomethingSelected = selectedList != null && selectedList.index != -1;
                var isDecisionSelected = isSomethingSelected && selectedList == decisionsList;
                var isActionSelected = isSomethingSelected && selectedList == decisionActionsList;
                
                
                rect.width = 20;
                EditorGUI.PropertyField(rect, enabled, GUIContent.none);
                rect.width = elementRect.width;
                
                EditorGUI.BeginDisabledGroup(!enabled.boolValue);

                rect.x += 20;
                rect.width -= 20;
                
                decisionsList.DoList(rect);
                rect.y += lineHeight * Mathf.Max(decisions.arraySize, 1) + verticalSpacing * 2 + 1;

                var addRemoveRect = new Rect(rect) { width = 20, height = 20 };
                
                addRemoveRect.x += 5;
                addRemoveRect.y += 5;
                if (GUI.Button(addRemoveRect, Content.plusMore, Styles.preButton))
                {
                    transitionAddTarget = transition;
                    lastPlusIndex = index;
                    
                    CreateTransitionAddMenu();
                    transitionAddMenu.DropDown(addRemoveRect);
                }
                
                EditorGUI.BeginDisabledGroup(!isDecisionSelected && !isActionSelected);
                
                addRemoveRect.x += 20;
                if (GUI.Button(addRemoveRect, Content.minus, Styles.preButton))
                {
                    if(isDecisionSelected)
                        decisions.DeleteArrayElementAtIndex(selectedList.index);
                    else if(isActionSelected)
                        actions.DeleteArrayElementAtIndex(selectedList.index);
                }
                EditorGUI.EndDisabledGroup();
                
                rect.x += 50;
                rect.width -= 50;
                decisionActionsList.DoList(rect);
                
                EditorGUI.EndDisabledGroup();
            };
            
            CreateDecisionsGUI();
            CreateDecisionActionsGUI();
        }

        private void CreateDecisionsGUI()
        {
            decisionsLists = new ReorderableList[transitionsProperty.arraySize];
            
            for (int i = 0; i < transitionsProperty.arraySize; i++)
            {
                var transition = transitionsProperty.GetArrayElementAtIndex(i);
                var decisions = transition.FindPropertyRelative("decisions");

                var list = new ReorderableList(serializedObject, decisions, true, false, false, false)
                {
                    elementHeight = singleLineHeight + standardVerticalSpacing,
                    headerHeight = 1
                };
                
                list.onSelectCallback += reorderableList =>
                {
                    selectedList = reorderableList;
                };
                
                list.drawElementCallback += (rect, index, active, focused) =>
                {
                    var element = decisions.GetArrayElementAtIndex(index);
                    var condition = element.FindPropertyRelative("condition");
                    var state = element.FindPropertyRelative("state");

                    rect.height = singleLineHeight;
                    rect.y += standardVerticalSpacing / 2;
                    
                    var conditionRect = new Rect(rect) { width = 55 };
                    EditorStyles.popup.fontSize--;
                    EditorGUI.PropertyField(conditionRect, condition, GUIContent.none);
                    EditorStyles.popup.fontSize++;

                    rect.x += conditionRect.width;
                    rect.width -= conditionRect.width;
                    EditorGUI.PropertyField(rect, state, GUIContent.none);
                };
                decisionsLists[i] = list;
            }
        }

        private void CreateDecisionActionsGUI()
        {
            transitionActionLists = new ReorderableList[transitionsProperty.arraySize];
            
            for (int i = 0; i < transitionsProperty.arraySize; i++)
            {
                var transition = transitionsProperty.GetArrayElementAtIndex(i);
                var actions = transition.FindPropertyRelative("actions");
                var nextState = transition.FindPropertyRelative("nextState");

                var list = new ReorderableList(serializedObject, actions, true, false, false, false)
                {
                    elementHeight = singleLineHeight + standardVerticalSpacing,
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

                    rect.height = singleLineHeight;
                    rect.y += standardVerticalSpacing / 2;
                    
                    EditorStyles.popup.fontSize--;
                    
                    var triggerRect = new Rect(rect) { width = 55 };
                    EditorGUI.PropertyField(triggerRect, trigger, GUIContent.none);
                    rect.x += triggerRect.width;
                    rect.width -= triggerRect.width;
                    
                    EditorStyles.popup.fontSize++;
                    
                    var iconRect = new Rect(rect) { width = 24, height = 20 };
                    rect.x += iconRect.width;
                    rect.width -= iconRect.width;
                    
                    var actionType = (StateTransition.ActionType)type.enumValueIndex;
                    switch (actionType)
                    {
                        case StateTransition.ActionType.Action:
                            GUI.Label(iconRect, Icons.action, Styles.iconLabel);
                            EditorGUI.PropertyField(rect, state, GUIContent.none);
                            break;
                            
                        case StateTransition.ActionType.ChangeState:
                            GUI.Label(iconRect, Icons.stateChange, Styles.iconLabel);
                            EditorGUI.PropertyField(rect, nextState, GUIContent.none);
                            break;
                            
                        default:
                            GUI.Label(rect, "No GUI implemented.");
                            break;
                    }
                };
                transitionActionLists[i] = list;
            }
        }
    }
}