using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GameSceneSO), true)]
public class GameSceneEditor : Editor
{
	#region UI_Warnings

	private const string noScenesWarning = "There are no scenes set for this level yet! Add a new scene with the dropdown below";

	#endregion

	#region GUI_Styles

	private GUIStyle headerLabelStyle;

	#endregion

	// it holds a list of properties to hide from basic inspector
	private static readonly string[] ExcludedProperties = { "m_Script", "sceneName" };

	private string[] sceneList;
	private GameSceneSO gameSceneTarget;

	private void OnEnable()
	{
		gameSceneTarget = target as GameSceneSO;
		PopulateScenePicker();
		InitializeGuiStyles();
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("Scene information", headerLabelStyle);
		EditorGUILayout.Space();
		DrawScenePicker();
		DrawPropertiesExcluding(serializedObject, ExcludedProperties);
	}

	private void DrawScenePicker()
	{
		var sceneName = gameSceneTarget.sceneName;
		EditorGUI.BeginChangeCheck();
		var selectedScene = sceneList.ToList().IndexOf(sceneName);

		if (selectedScene < 0)
		{
			EditorGUILayout.HelpBox(noScenesWarning, MessageType.Warning);
		}

		selectedScene = EditorGUILayout.Popup("Scene", selectedScene, sceneList);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Changed selected scene");
			gameSceneTarget.sceneName = sceneList[selectedScene];
			MarkAllDirty();
		}
	}

	private void InitializeGuiStyles()
	{
		headerLabelStyle = new GUIStyle(EditorStyles.largeLabel)
		{
			fontStyle = FontStyle.Bold,
			fontSize = 18,
			fixedHeight = 70.0f
		};
	}

	/// <summary>
	/// Populates the scene picker with scenes included in the game's build index
	/// </summary>
	private void PopulateScenePicker()
	{
		var sceneCount = SceneManager.sceneCountInBuildSettings;
		sceneList = new string[sceneCount];
		for (int i = 0; i < sceneCount; i++)
		{
			sceneList[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
		}
	}

	/// <summary>
	/// Marks scenes as dirty so data can be saved
	/// </summary>
	private void MarkAllDirty()
	{
		EditorUtility.SetDirty(target);
		EditorSceneManager.MarkAllScenesDirty();
	}
}
