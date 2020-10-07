using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KarimCastagnini.PluggableFSM
{
	[CustomEditor(typeof(StateBase), true)]
	public class StateEditor : Editor
	{
		private StateBase _target;
		private ReorderableList[] _lists;
		private int _currentListIndex;
		private Type _actionType;

		private void OnEnable()
		{
			_target = (StateBase)serializedObject.targetObject;
			_actionType = _target.GetActionType();

			_lists = new ReorderableList[3];

			InitializeList(0, "_onEnter", "On Enter Actions:");
			InitializeList(1, "_onExit", "On Exit Actions:");
			InitializeList(2, "_onUpdate", "On Update Actions:");
		}

		public override void OnInspectorGUI()
		{
			bool playing = EditorApplication.isPlayingOrWillChangePlaymode || Application.isPlaying;

			EditorGUI.BeginDisabledGroup(playing);
			serializedObject.Update(); //get the latest target version
			DrawInspector();
			serializedObject.ApplyModifiedProperties(); //save changes
			EditorGUI.EndDisabledGroup();

			if (playing)
				EditorGUILayout.HelpBox("Editing during PlayMode currently not supported.", MessageType.Info);
		}

		private void DrawInspector()
		{
			DrawList(0);
			EditorGUILayout.Space(5);
			DrawList(1);
			EditorGUILayout.Space(5);
			DrawList(2);
		}

		private void DrawList(int listIndex)
		{
			_currentListIndex = listIndex;
			_lists[_currentListIndex].DoLayoutList();
		}

		private void InitializeList(int listIndex, string propertyName, string headerName)
		{
			ReorderableList list = new ReorderableList(
				serializedObject,
				serializedObject.FindProperty(propertyName),
				true, true, true, true);

			list.drawElementCallback = DrawActionEntry;
			list.drawHeaderCallback = (rect) => GUI.Label(rect, headerName);

			_lists[listIndex] = list;
		}

		private void DrawActionEntry(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty element = _lists[_currentListIndex].serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;

			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = rect.width / 20f;

			Rect rectPos = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);

			element.objectReferenceValue =
				EditorGUI.ObjectField(rectPos, element.objectReferenceValue, _actionType, false) as Object;

			EditorGUIUtility.labelWidth = labelWidth;
		}
	}
}
