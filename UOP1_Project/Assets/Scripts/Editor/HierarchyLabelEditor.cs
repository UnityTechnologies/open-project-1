using UnityEditor;
using UnityEngine;

namespace UOP1.Tools
{
	/// <summary>
	/// Custom hierarchy label editor to show in the inspector.
	/// </summary>
	/// <remarks>
	/// Displays either instance or shared label property fields, depending on which one is currently used.
	/// </remarks>
	[CustomEditor(typeof(HierarchyLabel))]
	public class HierarchyLabelEditor : Editor
	{
		private SerializedProperty _text;
		private SerializedProperty _textColor;
		private SerializedProperty _backgroundColor;
		private SerializedProperty _sharedData;
		private SerializedProperty _labelDescription;

		private void OnEnable()
		{
			_text = serializedObject.FindProperty("_text");
			_textColor = serializedObject.FindProperty("_textColor");
			_backgroundColor = serializedObject.FindProperty("_backgroundColor");
			_sharedData = serializedObject.FindProperty("_sharedData");
			_labelDescription = serializedObject.FindProperty("_labelDescription");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(_sharedData);

			if (_sharedData.objectReferenceValue != null)
			{
				// Get shared data properties and display those instead of instance properties.
				SerializedObject data = new SerializedObject(_sharedData.objectReferenceValue);
				SerializedProperty text = data.FindProperty("text");
				SerializedProperty textColor = data.FindProperty("textColor");
				SerializedProperty backgroundColor = data.FindProperty("backgroundColor");
				SerializedProperty labelDescription = data.FindProperty("labelDescription");
				EditorGUILayout.PropertyField(text);
				EditorGUILayout.PropertyField(textColor);
				EditorGUILayout.PropertyField(backgroundColor);
				EditorGUILayout.PropertyField(labelDescription);
				data.ApplyModifiedProperties();
			}
			else
			{
				EditorGUILayout.PropertyField(_text);
				EditorGUILayout.PropertyField(_textColor);
				EditorGUILayout.PropertyField(_backgroundColor);
				EditorGUILayout.PropertyField(_labelDescription);
			}

			serializedObject.ApplyModifiedProperties();

			if (GUI.changed)
			{
				EditorApplication.RepaintHierarchyWindow();
			}

			EditorGUILayout.HelpBox("This script displays a custom label for this game object in the hierarchy " +
				"window, to help separate and identify scene content when working in the editor.", MessageType.Info);
		}

		[MenuItem("GameObject/Create Hierarchy Label", false, 0)]
		private static void CreateHierarchyLabel(MenuCommand menuCommand)
		{
			GameObject go = new GameObject("Label");
			go.tag = "EditorOnly";
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
