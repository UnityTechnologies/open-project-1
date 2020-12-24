using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property,
											GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	public override void OnGUI(Rect position,
							   SerializedProperty property,
							   GUIContent label)
	{
		
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, false);
		GUI.enabled = true;
	}
}
