using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UOP1.StateMachine.ScriptableObjects;
using static UnityEditor.EditorGUILayout;

namespace UOP1.StateMachine.Editor
{
	internal class AddTransitionHelper : IDisposable
	{
		internal SerializedTransition SerializedTransition { get; }
		private readonly SerializedObject _transition;
		private readonly ReorderableList _list;
		private readonly TransitionTableEditor _editor;
		private bool _toggle = false;

		internal AddTransitionHelper(TransitionTableEditor editor)
		{
			_editor = editor;
			_transition = new SerializedObject(ScriptableObject.CreateInstance<TransitionItemSO>());
			SerializedTransition = new SerializedTransition(_transition.FindProperty("Item"));
			_list = new ReorderableList(_transition, SerializedTransition.Conditions);
			SetupConditionsList(_list);
		}

		internal void Display()
		{
			// Display add button only if not already adding a transition
			if (!_toggle)
			{
				if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus"), GUILayout.Width(35)))
				{
					_toggle = true;
					SerializedTransition.ClearProperties();
				}

				if (!_toggle)
					return;
			}

			var rect = BeginVertical();
			rect.x += 45;
			rect.width -= 40;
			EditorGUI.DrawRect(rect, ContentStyle.LightGray);
			Separator();

			// State Fields
			BeginHorizontal();
			{
				Space(50, false);

				StatePropField("From", SerializedTransition.FromState);
				Space(10, false);
				StatePropField("To", SerializedTransition.ToState);
			}
			EndHorizontal();

			// Conditions List
			BeginHorizontal();
			{
				Space(50, false);
				BeginVertical();
				_list.DoLayoutList();
				EndVertical();
			}
			EndHorizontal();

			Separator();

			// Add and cancel buttons
			BeginHorizontal();
			{
				Space(50, false);

				if (GUILayout.Button("Add Transition"))
				{
					if (SerializedTransition.FromState.objectReferenceValue == null)
						Debug.LogException(new ArgumentNullException("FromState"));
					else if (SerializedTransition.ToState.objectReferenceValue == null)
						Debug.LogException(new ArgumentNullException("ToState"));
					else if (SerializedTransition.FromState.objectReferenceValue == SerializedTransition.ToState.objectReferenceValue)
						Debug.LogException(new InvalidOperationException("FromState and ToState are the same."));
					else
					{
						_editor.AddTransition(SerializedTransition);
						_toggle = false;
					}
				}
				else if (GUILayout.Button("Cancel"))
				{
					_toggle = false;
				}
			}
			EndHorizontal();
			EndVertical();

			void StatePropField(string label, SerializedProperty prop)
			{
				BeginVertical();
				LabelField(label);
				BeginHorizontal();
				Space(20, false);
				PropertyField(prop, GUIContent.none, GUILayout.MaxWidth(180));
				EndHorizontal();
				EndVertical();
			}
		}

		public void Dispose()
		{
			UnityEngine.Object.DestroyImmediate(_transition.targetObject);
			_transition.Dispose();
			GC.SuppressFinalize(this);
		}

		private static void SetupConditionsList(ReorderableList reorderableList)
		{
			reorderableList.elementHeight *= 2.3f;
			reorderableList.drawHeaderCallback += rect => GUI.Label(rect, "Conditions");
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
				if (condition.objectReferenceValue != null)
				{
					string label = condition.objectReferenceValue.name;
					GUI.Label(rect, "If");
					GUI.Label(new Rect(rect.x + 20, rect.y, rect.width, rect.height), label, EditorStyles.boldLabel);
					EditorGUI.PropertyField(new Rect(rect.x + rect.width - 180, rect.y, 20, rect.height), condition, GUIContent.none);
				}
				else
				{
					EditorGUI.PropertyField(new Rect(rect.x, rect.y, 150, rect.height), condition, GUIContent.none);
				}
				EditorGUI.LabelField(new Rect(rect.x + rect.width - 120, rect.y, 20, rect.height), "Is");
				EditorGUI.PropertyField(new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height), prop.FindPropertyRelative("ExpectedResult"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(rect.x + 20, rect.y + EditorGUIUtility.singleLineHeight + 5, 60, rect.height), prop.FindPropertyRelative("Operator"), GUIContent.none);
			};

			reorderableList.onChangedCallback += list => reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
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

		// SO to serialize a TransitionItem
		internal class TransitionItemSO : ScriptableObject
		{
			public TransitionTableSO.TransitionItem Item = default;
		}
	}
}
