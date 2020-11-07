/*

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// <see cref="DialogueDataSO"/> editor that draw default inspector except:
/// <list type="bullet">
/// <item><see cref="DialogueDataSO.Conversation"/></item>.
/// </list>
/// </summary>

[CustomEditor(typeof(DialogueDataSO))]
public class DialogueDataEditor : Editor
{
	private readonly Type _dialogueData = typeof(DialogueDataSO);

	/// <summary>
	/// All serialized fields in <see cref="DialogueDataSO"/>
	/// </summary>
	private readonly List<SerializedProperty> _serializedFields = new List<SerializedProperty>();
	private ReorderableList _dialogueList;


	private void OnEnable()
	{
		PrepareSerializedProperties();
	}

	public override void OnInspectorGUI()
	{ 
		DrawCustomInspector();
	}


	private void DrawCustomInspector()
	{
		// Make GUI not changeable.
		GUI.enabled = false;

		// Draw reference information about script being edited.
		EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((DialogueDataSO)target), typeof(DialogueDataSO), false);

		// Make GUI changeable
		GUI.enabled = true;

		serializedObject.Update();

		// Draw field to display
		foreach (SerializedProperty field in _serializedFields)
		{
			if (field != null)
			{
				if (field.name == nameof(DialogueDataSO.Conversation))
				{
					_dialogueList.DoLayoutList();
				}
				else  // Draw Default property fields.
				{
					EditorGUILayout.PropertyField(field);
				}
			}
		}

		serializedObject.ApplyModifiedProperties();
	}

	/// <summary>
	/// Identify all serialized property from selected <see cref="DialogueDataSO"/>
	/// </summary>
	private void PrepareSerializedProperties()
	{
		// Prepare serialized property.
		FieldInfo[] fields = _dialogueData.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		foreach (FieldInfo field in fields)
		{
			SerializedProperty serializedProperty = serializedObject.FindProperty(field.Name);

			_serializedFields.Add(serializedProperty);

			if (field.Name == nameof(DialogueDataSO.Conversation))
			{
				_dialogueList = new ReorderableList(serializedProperty);
			}
		}
	}
}

*/
