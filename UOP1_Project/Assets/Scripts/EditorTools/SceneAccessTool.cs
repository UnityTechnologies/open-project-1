using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class SceneAccessTool : EditorWindow, IHasCustomMenu
{
	private SceneAccessHolderSO _inspected;
	private SerializedObject _serializedObject;
	private Editor _sceneAccessHolderSOEditor;
	public enum Layout { List, Grid }
	private Layout _layout;
	private bool _showOptions = false;
	private bool _editMode = false;
	private ReorderableList list;

	private void OnEnable()
	{

		_inspected = Resources.Load<SceneAccessHolderSO>("SceneAccessHolder");
		SetUpList();
	}
	private void SetUpList()
	{
		_serializedObject = new SerializedObject(_inspected);

		if (_editMode)
		{
			list = new ReorderableList(_serializedObject,
				_serializedObject.FindProperty("sceneList"),
				true, false, false, false);
			list.drawElementCallback =
				(Rect rect, int index, bool isActive, bool isFocused) =>
				{
					var element = list.serializedProperty.GetArrayElementAtIndex(index);
					rect.y += 2;
					EditorGUI.PropertyField(
						new Rect(rect.x, rect.y, rect.width - 30, EditorGUIUtility.singleLineHeight),
						element.FindPropertyRelative("name"), GUIContent.none);

					EditorGUI.PropertyField(
						new Rect(rect.x + rect.width - 25, rect.y, 25, EditorGUIUtility.singleLineHeight),
						element.FindPropertyRelative("visible"), GUIContent.none);
				};
		}
	}
	[MenuItem("Tools/SceneAccessTool")]
	public static void ShowWindow()
	{
		GetWindow<SceneAccessTool>("SceneAccessTool");
	}
	private void OnGUI()
	{
		if (_inspected != null)
		{
			if (_showOptions)
				ShowOptions();
			ShowSceneList();
		}
	}

	public void ShowOptions()
	{
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Populate Scene List"))
		{
			PopulateSceneList();
		}
		if (GUILayout.Button("Clear List"))
		{
			ClearSceneList();
		}
		if (GUILayout.Button("Edit Mode"))
		{
			ToggleEditMode();
		}
		GUILayout.EndHorizontal();
	}
	public void ShowSceneList()
	{
		if (_editMode)
		{
			_serializedObject.Update();
			list.DoLayoutList();
			_serializedObject.ApplyModifiedProperties();
		}
		else
		{
			if (_layout == Layout.List)
			{
				for (int i = 0; i < _inspected.sceneList.Count; i++)
				{
					var sceneItem = _inspected.sceneList[i];
					if (!sceneItem.visible)
					{
						continue;
					}
					if (GUILayout.Button(sceneItem.name))
					{
						EditorSceneManager.OpenScene(sceneItem.path);
					}
				}
			}
			else
			{
				int gridSize = 48;
				int widthCount = gridSize;
				GUILayout.BeginHorizontal();
				for (int i = 0; i < _inspected.sceneList.Count; i++)
				{
					var sceneItem = _inspected.sceneList[i];
					if (!sceneItem.visible)
					{
						continue;
					}

					GUIStyle customButton = new GUIStyle("button");
					customButton.fontSize = 10;
					customButton.alignment = TextAnchor.UpperLeft;
					customButton.wordWrap = true;
					GUIContent guiContent = new GUIContent(sceneItem.name);
					if (GUILayout.Button(
						guiContent,
						customButton,
						GUILayout.Width(gridSize),
						GUILayout.Height(gridSize)))
					{
						EditorSceneManager.OpenScene(sceneItem.path);
					}

					widthCount += gridSize + 4;

					if (widthCount > position.width)
					{
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						widthCount = gridSize;
					}
				}

				GUILayout.EndHorizontal();
			}

		}
	}

	/// <summary>
	/// Find all scenes in the project and put them in the list
	/// </summary>
	private void PopulateSceneList()
	{
		EditorBuildSettingsScene[] currentScenes = EditorBuildSettings.scenes;
		EditorBuildSettingsScene[] filteredScenes = currentScenes.Where(ebss => File.Exists(ebss.path)).ToArray();
		List<SceneAccessHolderSO.SceneInfo> allScene = new List<SceneAccessHolderSO.SceneInfo>();
		for (int i = 0; i < filteredScenes.Length; i++)
		{
			allScene.Add(
				new SceneAccessHolderSO.SceneInfo
				{
					name = Path.GetFileNameWithoutExtension(filteredScenes[i].path),
					path = Path.GetFullPath(filteredScenes[i].path),
					visible = true
				});
		}
		//add the new scenes
		foreach (SceneAccessHolderSO.SceneInfo sceneInfo in allScene)
		{
			if (!_inspected.sceneList.Contains(sceneInfo))
			{
				_inspected.sceneList.Add(sceneInfo);
			}
		}
		//remove the deleted scenes
		foreach (SceneAccessHolderSO.SceneInfo sceneInfo in _inspected.sceneList.ToList())
		{
			if (!allScene.Contains(sceneInfo))
			{
				_inspected.sceneList.Remove(sceneInfo);
			}
		}
	}

	private void ClearSceneList()
	{
		_inspected.sceneList.Clear();
	}

	public void AddItemsToMenu(GenericMenu menu)
	{
		menu.AddItem(new GUIContent("ToggleOptions"), false, ToggleOptions);
		menu.AddItem(new GUIContent("ToggleLayout"), false, ToggleLayout);
	}

	private void ToggleOptions()
	{
		_showOptions = !_showOptions;
	}
	private void ToggleLayout()
	{
		if (_layout == Layout.List)
		{
			_layout = Layout.Grid;
		}
		else
		{
			_layout = Layout.List;
		}
	}
	private void ToggleEditMode()
	{
		_editMode = !_editMode;
		SetUpList();
	}
}
