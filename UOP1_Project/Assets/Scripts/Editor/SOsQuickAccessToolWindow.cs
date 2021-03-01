using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

class SOsQuickAccessToolWindow : EditorWindow
{
	[Header("Editor Window Related")]
	Vector2 scroll;
	int selected;

	[Header("SOs related")]
	string[] assetSearchFolders;

	List<string> SOTypes;
	string[] objectsGUIDs;
	string[] objectsPaths;
	ScriptableObject[] objects;

	string[] displayObjectsGUIDs;
	List<string> displayObjectsPaths;
	List<ScriptableObject> displayObjects;

	private void OnEnable()
	{
		assetSearchFolders = new string[1];
		assetSearchFolders[0] = "Assets/ScriptableObjects";

		FindAllSOs();
		FindDisplaySOs();
	}

	void OnFocus()
	{
		FindAllSOs();
		FindDisplaySOs();
	}

	[MenuItem("Tools/SOs Quick Access Tool")]
	private static void ShowWindow()
	{
		GetWindow<SOsQuickAccessToolWindow>("SOs Quick Access Tool");
	}

	void OnGUI()
	{
		GUILayout.Space(EditorGUIUtility.singleLineHeight * 0.5f);

		GUILayout.BeginHorizontal();

		DrawSOsPicker();
		if (GUILayout.Button("Refresh All"))
		{
			FindAllSOs();
			FindDisplaySOs();
		}

		GUILayout.EndHorizontal();

		DrawSOsList();
	}

	void DrawSOsPicker()
	{
		EditorGUI.BeginChangeCheck();
		selected = EditorGUILayout.Popup(GUIContent.none, selected, SOTypes.ToArray());
		if (EditorGUI.EndChangeCheck())
		{
			FindDisplaySOs();
		}
	}

	void DrawSOsList()
	{
		scroll = GUILayout.BeginScrollView(scroll);

		for (int i = 0; i < displayObjectsGUIDs.Length; i++)
		{
			GUILayout.Label(i + 1 + ". " + displayObjects[i].name);

			if (GUILayout.Button("Locate Quickly"))
			{
				EditorUtility.FocusProjectWindow();
				EditorGUIUtility.PingObject(displayObjects[i]);
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
		}

		GUILayout.EndScrollView();
	}

	void FindAllSOs()
	{
		objectsGUIDs = AssetDatabase.FindAssets("t:ScriptableObject", assetSearchFolders) as string[];

		objectsPaths = new string[objectsGUIDs.Length];
		objects = new ScriptableObject[objectsGUIDs.Length];

		SOTypes = new List<string>();

		for (int i = 0; i < objectsGUIDs.Length; i++)
		{
			objectsPaths[i] = AssetDatabase.GUIDToAssetPath(objectsGUIDs[i]);
			objects[i] = (ScriptableObject)AssetDatabase.LoadAssetAtPath(objectsPaths[i], typeof(ScriptableObject));
			//Debug.Log(objectsGUIDs[i] + ": " + objectsPaths[i] + " - " + i);
		}

		for (int i = 0; i < objects.Length; i++)
		{
			if (SOTypes.IndexOf(objects[i].GetType().ToString()) == -1)
			{
				SOTypes.Add(objects[i].GetType().ToString());
			}
		}
	}

	void FindDisplaySOs()
	{
		if (displayObjects != null)
		{
			displayObjects.Clear();
		}
		if (displayObjectsPaths != null)
		{
			displayObjectsPaths.Clear();
		}

		string type = SOTypes[selected];
		string queryString = "t:" + type;

		displayObjectsGUIDs = AssetDatabase.FindAssets(queryString);

		displayObjectsPaths = new List<string>(displayObjectsGUIDs.Length);
		displayObjects = new List<ScriptableObject>(displayObjectsGUIDs.Length);

		for (int i = 0; i < displayObjectsGUIDs.Length; i++)
		{
			displayObjectsPaths.Add(AssetDatabase.GUIDToAssetPath(displayObjectsGUIDs[i]));
			displayObjects.Add(AssetDatabase.LoadAssetAtPath(displayObjectsPaths[i], typeof(ScriptableObject)) as ScriptableObject);
		}
	}
}
