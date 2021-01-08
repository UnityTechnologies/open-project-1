using System;
using System.Collections.Generic;
using UnityEngine;

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
