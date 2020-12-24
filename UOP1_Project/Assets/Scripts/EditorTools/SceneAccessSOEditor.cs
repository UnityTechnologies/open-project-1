using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(SceneAccessHolderSO))]
public class SceneAccessSOEditor : Editor
{
	public static event System.Action OnChangeSO;

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();

		// Show default inspector property editor
		DrawDefaultInspector();

		if (EditorGUI.EndChangeCheck())
		{
			OnChangeSO?.Invoke();
			//Other variant to send meassage from  SceneAccessSOEditor to SceneAccessTool
			//SceneAccessTool.action?.Invoke();
		}
	}
}
