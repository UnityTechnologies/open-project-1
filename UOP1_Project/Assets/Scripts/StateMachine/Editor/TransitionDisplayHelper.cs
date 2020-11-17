using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UOP1.StateMachine.Editor
{
	internal class TransitionDisplayHelper
	{
		internal SerializedTransition SerializedTransition { get; }
		private readonly ReorderableList _reorderableList;
		private readonly TransitionTableEditor _editor;

		internal TransitionDisplayHelper(SerializedTransition serializedTransition, TransitionTableEditor editor)
		{
			SerializedTransition = serializedTransition;
			_reorderableList = new ReorderableList(SerializedTransition.Transition.serializedObject, SerializedTransition.Conditions, true, false, true, true);
			SetupConditionsList(_reorderableList);
			_editor = editor;
		}

		internal bool Display()
		{
			// Transition Header
			EditorGUI.DrawRect(EditorGUILayout.BeginHorizontal(GUILayout.Height(24)), ContentStyle.DarkGray);
			{
				// Target state
				EditorGUILayout.Space(3f, false);
				EditorGUILayout.LabelField("To", GUILayout.Width(20));
				EditorGUILayout.LabelField(SerializedTransition.ToState.objectReferenceValue.name, EditorStyles.boldLabel);

				// TODO: Fix the space in between the labels above and the buttons below
				// Right now the buttons disappear to the right if the Inspector is made too narrow

				// Move transition up
				if (GUILayout.Button(EditorGUIUtility.IconContent("scrollup"), GUILayout.Width(30), GUILayout.Height(18)))
				{
					if (_editor.ReorderTransition(SerializedTransition, true))
						return true;
				}
				// Move transition down
				if (GUILayout.Button(EditorGUIUtility.IconContent("scrolldown"), GUILayout.Width(30), GUILayout.Height(18)))
				{
					if (_editor.ReorderTransition(SerializedTransition, false))
						return true;
				}
				// Remove transition
				if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus"), GUILayout.Width(30), GUILayout.Height(18)))
				{
					_editor.RemoveTransition(SerializedTransition.Index);
					return true;
				}
			}
			EditorGUILayout.EndHorizontal();

			// Conditions
			_reorderableList.DoLayoutList();

			return false;
		}

		private static void SetupConditionsList(ReorderableList reorderableList)
		{
			reorderableList.elementHeight *= 2.3f;
			reorderableList.headerHeight = 1f;
			reorderableList.onAddCallback += list =>
			{
				int count = list.count;
				list.serializedProperty.InsertArrayElementAtIndex(count);
				var prop = list.serializedProperty.GetArrayElementAtIndex(count);
				prop.FindPropertyRelative("Condition").objectReferenceValue = null;
				prop.FindPropertyRelative("ExpectedResult").enumValueIndex = 0;
				prop.FindPropertyRelative("Operator").enumValueIndex = 0;
			};

			reorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var prop = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
				rect = new Rect(rect.x, rect.y + 2.5f, rect.width, EditorGUIUtility.singleLineHeight);
				var condition = prop.FindPropertyRelative("Condition");

				// Draw the picker for the Condition SO
				if (condition.objectReferenceValue != null)
				{
					string label = condition.objectReferenceValue.name;
					GUI.Label(rect, "If");
					var r = rect;
					r.x += 20;
					r.width = 35;
					EditorGUI.PropertyField(r, condition, GUIContent.none);
					r.x += 40;
					r.width = rect.width - 120;
					GUI.Label(r, label, EditorStyles.boldLabel);
				}
				else
				{
					EditorGUI.PropertyField(new Rect(rect.x, rect.y, 150, rect.height), condition, GUIContent.none);
				}

				// Draw the boolean value expected by the condition (i.e. "Is True", "Is False")
				EditorGUI.LabelField(new Rect(rect.x + rect.width - 80, rect.y, 20, rect.height), "Is");
				EditorGUI.PropertyField(new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height), prop.FindPropertyRelative("ExpectedResult"), GUIContent.none);

				// Only display the logic condition if there's another one after this
				if (index < reorderableList.count - 1)
					EditorGUI.PropertyField(new Rect(rect.x + 20, rect.y + EditorGUIUtility.singleLineHeight + 5, 60, rect.height), prop.FindPropertyRelative("Operator"), GUIContent.none);
			};

			reorderableList.onChangedCallback += list => list.serializedProperty.serializedObject.ApplyModifiedProperties();
			reorderableList.drawElementBackgroundCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				if (isFocused)
					EditorGUI.DrawRect(rect, ContentStyle.Focused);

				if (index % 2 != 0)
					EditorGUI.DrawRect(rect, ContentStyle.ZebraDark);
				else
					EditorGUI.DrawRect(rect, ContentStyle.ZebraLight);
			};
		}
	}
}
