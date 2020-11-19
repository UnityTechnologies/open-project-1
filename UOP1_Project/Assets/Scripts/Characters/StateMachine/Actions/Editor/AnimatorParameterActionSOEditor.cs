using UnityEditor;
using UnityEngine;
using UOP1.StateMachine;

[CustomEditor(typeof(AnimatorParameterActionSO)), CanEditMultipleObjects]
public class AnimatorParameterActionSOEditor : UnityEditor.Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("_whenToRun"));
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Animator Parameter", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("_parameterName"), new GUIContent("Name"));

		// Draws the appropriate value depending on the type of parameter this SO is going to change on the Animator
		SerializedProperty animParamValue = serializedObject.FindProperty("_parameterType");

		EditorGUILayout.PropertyField(animParamValue, new GUIContent("Type"));

		switch (animParamValue.intValue)
		{
			case (int)AnimatorParameterActionSO.ParameterType.Bool:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_boolValue"), new GUIContent("Target value"));
				break;
			case (int)AnimatorParameterActionSO.ParameterType.Int:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_intValue"), new GUIContent("Target value"));
				break;
			case (int)AnimatorParameterActionSO.ParameterType.Float:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_floatValue"), new GUIContent("Target value"));
				break;

		}

		serializedObject.ApplyModifiedProperties();
	}
}
