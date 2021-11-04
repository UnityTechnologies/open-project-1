using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom base editor class with handy methods for GUI drawing.
/// </summary>
public class CustomBaseEditor : Editor
{
	/// <summary>
	/// Draw the default, non-editable script field. Useful when creating a custom Inspector but we want it to look like a default one.
	/// Plus, it's handy to be able to click on the field to ping the Script in the Project window.
	/// </summary>
	/// <typeparam name="T">Inspected type.</typeparam>
	public void DrawNonEditableScriptReference<T>() where T : Object
	{
		GUI.enabled = false;

		if (typeof(ScriptableObject).IsAssignableFrom(typeof(T)))
			EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(T), false);
		else if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
			EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(T), false);

		GUI.enabled = true;
	}
}
