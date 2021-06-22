using UnityEditor;
using UnityEngine;

namespace UOP1.Tools
{
	/// <summary>
	/// Custom hierarchy label editor to show in the inspector.
	/// </summary>
	/// <remarks>
	/// Hides unused fields to avoid confusion when switching between instance and shared data. 
	/// </remarks>
	[CustomEditor(typeof(HierarchyLabel))]
	public class HierarchyLabelEditor : Editor
	{
		private SerializedProperty _text;
		private SerializedProperty _textColor;
		private SerializedProperty _backgroundColor;
		private SerializedProperty _sharedData;

		private void OnEnable()
		{
			_text = serializedObject.FindProperty("_text");
			_textColor = serializedObject.FindProperty("_textColor");
			_backgroundColor = serializedObject.FindProperty("_backgroundColor");
			_sharedData = serializedObject.FindProperty("_sharedData");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (_sharedData.objectReferenceValue == null)
			{
				EditorGUILayout.PropertyField(_text);
				EditorGUILayout.PropertyField(_textColor);
				EditorGUILayout.PropertyField(_backgroundColor);
			}

			EditorGUILayout.PropertyField(_sharedData);

			serializedObject.ApplyModifiedProperties();

			if (GUI.changed)
			{
				EditorApplication.RepaintHierarchyWindow();
			}
		}

		[MenuItem("GameObject/Create Hierarchy Label", false, 0)]
		private static void CreateHierarchyLabel(MenuCommand menuCommand)
		{
			GameObject go = new GameObject("New Label");
			go.AddComponent<HierarchyLabel>();

			if (Selection.activeTransform == null)
			{
				// Place new label at top of hierarchy.
				go.transform.SetSiblingIndex(0);
			}
			else
			{
				// Place new label above current selection in hierarchy.
				go.transform.SetSiblingIndex(Selection.activeTransform.root.GetSiblingIndex());
			}

			Undo.RegisterCreatedObjectUndo(go, "Create Hierarchy Label");
		}
	}
}
