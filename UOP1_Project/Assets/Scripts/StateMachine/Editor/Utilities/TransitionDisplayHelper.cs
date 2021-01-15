using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.EditorGUI;

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

		internal bool Display(ref Rect position)
		{
			var rect = position;
			float listHeight = _reorderableList.GetHeight();
			float singleLineHeight = EditorGUIUtility.singleLineHeight;

			// Reserve space
			{
				rect.height = singleLineHeight + 10 + listHeight;
				GUILayoutUtility.GetRect(rect.width, rect.height);
				position.y += rect.height + 5;
			}

			// Background
			{
				rect.x += 5;
				rect.width -= 10;
				rect.height -= listHeight;
				DrawRect(rect, ContentStyle.DarkGray);
			}

			// Transition Header
			{
				rect.x += 3;
				LabelField(rect, "To");

				rect.x += 20;
				LabelField(rect, SerializedTransition.ToState.objectReferenceValue.name, EditorStyles.boldLabel);
			}


			// Buttons
			{
				bool Button(Rect pos, string icon) => GUI.Button(pos, EditorGUIUtility.IconContent(icon));

				var buttonRect = new Rect(x: rect.width - 25, y: rect.y + 5, width: 30, height: 18);

				int i, l;
				{
					var transitions = _editor.GetStateTransitions(SerializedTransition.FromState.objectReferenceValue);
					l = transitions.Count - 1;
					i = transitions.FindIndex(t => t.Index == SerializedTransition.Index);
				}

				// Remove transition
				if (Button(buttonRect, "Toolbar Minus"))
				{
					_editor.RemoveTransition(SerializedTransition);
					return true;
				}
				buttonRect.x -= 35;

				// Move transition down
				if (i < l)
				{
					if (Button(buttonRect, "scrolldown"))
					{
						_editor.ReorderTransition(SerializedTransition, false);
						return true;
					}
					buttonRect.x -= 35;
				}

				// Move transition up
				if (i > 0)
				{
					if (Button(buttonRect, "scrollup"))
					{
						_editor.ReorderTransition(SerializedTransition, true);
						return true;
					}
					buttonRect.x -= 35;
				}

				// State editor
				if (Button(buttonRect, "SceneViewTools"))
				{
					_editor.DisplayStateEditor(SerializedTransition.ToState.objectReferenceValue);
					return true;
				}
			}

			rect.x = position.x + 5;
			rect.y += rect.height;
			rect.width = position.width - 10;
			rect.height = listHeight;

			// Display conditions
			_reorderableList.DoList(rect);

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
