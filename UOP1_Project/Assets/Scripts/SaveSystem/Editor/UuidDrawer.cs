using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Uuid))]
public class UuidDrawer : PropertyDrawer
{
	public override void OnGUI(Rect a_Position, SerializedProperty a_Property, GUIContent a_Label)
	{
		EditorGUI.BeginProperty(a_Position, a_Label, a_Property);

		a_Position = EditorGUI.PrefixLabel(a_Position, GUIUtility.GetControlID(FocusType.Passive), a_Label);

		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		const int uuidButtonWidth = 80;
		Rect uuidRect = new Rect(a_Position.x, a_Position.y, a_Position.width - uuidButtonWidth, a_Position.height);
		Rect newUuidButtonRect =
			new Rect(a_Position.x + uuidRect.width, a_Position.y, uuidButtonWidth, a_Position.height);

		SerializedProperty uuidProperty = a_Property.FindPropertyRelative("uuid");
		EditorGUI.SelectableLabel(uuidRect, uuidProperty.stringValue);
		if (GUI.Button(newUuidButtonRect, "New Uuid"))
		{
			if (EditorUtility.DisplayDialog("Generate new UUID?",
				"Generating a new UUID will break any data using the current uuid", "Yes", "Cancel"))
			{
				uuidProperty.stringValue = Guid.NewGuid().ToString();
			}
		}

		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
