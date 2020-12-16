using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneDatabase", menuName = "Scene Data/SceneDatabase")]
public class SceneDatabaseSO : ScriptableObject
{
	private static SceneDatabaseSO _instance;
	public static SceneDatabaseSO Instance { get { return _instance; } }
	public SceneSO[] Scenes;

	public SceneDatabaseSO()
	{
		_instance = this;
	}
	[RuntimeInitializeOnLoadMethod]
	private static void Initialize ()
	{
		_instance = Resources.LoadAll<SceneDatabaseSO>("Databases")[0];
	}
}
