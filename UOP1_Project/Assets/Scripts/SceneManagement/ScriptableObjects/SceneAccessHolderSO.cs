using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute
{

}

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
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}
}
/// <summary>
/// It holds a list of scenes that's shown in the scene quick access tool
/// </summary>
[CreateAssetMenu(fileName = "SceneAccessHolder", menuName = "Scene Data/SceneAccessHolder")]
public class SceneAccessHolderSO : ScriptableObject
{
	[Serializable]
	public struct SceneInfo : IEquatable<SceneInfo>
	{
		[ReadOnly] public string name;
		[ReadOnly] public string path;
		public bool visible;

		public bool Equals(SceneInfo other)
		{
			// Would still want to check for null etc. first.
			return this.path == other.path;
		}
	}

	[Header("List of Scenes")]
	public List<SceneInfo> sceneList;
}
