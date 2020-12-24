using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// It holds a list of scenes that's shown in the scene quick access tool
/// </summary>
[CreateAssetMenu(fileName = "SceneAccessHolder", menuName = "Scene Data/SceneAccessHolder")]
public class SceneAccessHolderSO : ScriptableObject
{
	[Serializable]
	public struct SceneInfo : IEquatable<SceneInfo>
	{
		[ReadOnly]
		public string name;
		[ReadOnly]
		public string path;
		public bool visible;

		public bool Equals(SceneInfo other)
		{
			// Not check on null because it's a sructure
			return this.path == other.path;
		}
	}

	[Header("Scene Access Tool Options")]
	//[HideInInspector]
	public bool showOptions;
	//[HideInInspector]
	public bool editMode;
	//[HideInInspector]
	public Layout sceneAccessLayout;

	[Header("List of Scenes")]
	public List<SceneInfo> sceneList;

	public SceneAccessHolderSO()
	{
		this.sceneList = new List<SceneInfo>();
	}
}
