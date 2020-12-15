using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(SceneAccessHolderSO), true)]
[CanEditMultipleObjects]
public class SceneAccessHolderSOEditor : Editor
{
	//private SceneAccessHolderSO _inspected;
	//private void OnEnable()
	//{
	//	_inspected = target as SceneAccessHolderSO;
	//}

	//public override void OnInspectorGUI()
	//{
	//	ShowOptions();
	//	ShowSceneList();
	//}
	//public void ShowOptions()
	//{
	//	GUILayout.BeginHorizontal();
	//	if (GUILayout.Button("Populate Scene List"))
	//	{
	//		PopulateSceneList();
	//	}
	//	if (GUILayout.Button("Clear Scene List"))
	//	{
	//		ClearSceneList();
	//	}
	//	GUILayout.EndHorizontal();
	//}
	//public void ShowSceneList()
	//{
	//	for (int i = 0; i < _inspected.sceneList.Count; i++)
	//	{
	//		GUILayout.Button(_inspected.sceneList[i]);
	//	}
	//}

	///// <summary>
	///// Find all scenes in the project and put them in the list
	///// </summary>
	//private void PopulateSceneList()
	//{
	//	ClearSceneList();
	//	var sceneCount = SceneManager.sceneCountInBuildSettings;
	//	for (int i = 0; i < sceneCount; i++)
	//	{
	//		_inspected.sceneList.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
	//	}
	//}

	//private void ClearSceneList()
	//{
	//	_inspected.sceneList.Clear();
	//}

}
