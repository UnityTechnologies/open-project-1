using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom base editor class. Inherited from Editor class.
/// Contains handy method to use.
/// </summary>
public class CustomBaseEditor : Editor
{
	/// <summary>
	/// Draw reference information about script being edited.
	/// </summary>
	/// <typeparam name="T">Inspected type.</typeparam>
	public void DrawNonEdtiableScriptReference<T>() where T : Object
	{
		//Make GUI not editable.
		GUI.enabled = false;

		//Draw reference information about script being edited
		if (typeof(ScriptableObject).IsAssignableFrom(typeof(T)))
			EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(T), false);
		else if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
			EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(T), false);

		//Make GUI editable
		GUI.enabled = true;
	}
}
