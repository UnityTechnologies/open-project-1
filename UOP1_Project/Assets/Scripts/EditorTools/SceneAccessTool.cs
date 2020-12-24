using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable] public enum Layout { List, Grid }

public class SceneAccessTool : EditorWindow, IHasCustomMenu
{
	private SceneAccessHolderSO _inspected;
	private SerializedObject _serializedObject;
	private Editor _sceneAccessHolderSOEditor;

	//Variables for store the style & layout for GUI Button in case of use the Grid Layout for SceneAccessTool
	private GUIStyle styleCustomButton;
	private const int gridSize = 48;
	private GUILayoutOption[] layoutOptionsCustomButton; 

	private Layout _layout = Layout.List;
	private bool _showOptions = false;
	private bool _editMode = false;
	private ReorderableList list;

	//Other variant to send meassage from  SceneAccessSOEditor to SceneAccessTool
	//public static Action action;

	private void OnEnable()
	{
		_inspected = Resources.Load<SceneAccessHolderSO>("SceneAccessHolder");

		if (_inspected != null)
			SetParametersSceneAccessToolFromSO();
		else
			{
				Debug.Log("Resource \"SceneAccessHolder\" not found, Created new");
				_inspected = CreateInstance<SceneAccessHolderSO>();
				AssetDatabase.CreateAsset(_inspected, "Assets/ScriptableObjects/SceneData/Resources/SceneAccessHolder.asset");
				AssetDatabase.SaveAssets();
				PopulateSceneList(true);
			}
		
		_serializedObject = new SerializedObject(_inspected);
		CreateSceneListInEditMode();
		SceneAccessSOEditor.OnChangeSO += SceneAccessSOEditor_OnChangeSO;
		//action = () => SceneAccessSOEditor_OnChangeSO();
	}

	private void SetParametersSceneAccessToolFromSO()
	{
		_showOptions = _inspected.showOptions;
		_editMode = _inspected.editMode;
		_layout = _inspected.sceneAccessLayout;
	}

	private void SceneAccessSOEditor_OnChangeSO()
	{
		SetParametersSceneAccessToolFromSO();
		Repaint();
	}

	[MenuItem("Tools/SceneAccessTool")]
	public static void ShowWindow()
	{
		
		GetWindow<SceneAccessTool>("SceneAccessTool");
	}

	private void OnGUI()
	{
		//Debug.Log("Current detected event: " + Event.current);
		//Set style & layout for GUI Button in case of use the Grid Layout for SceneAccessTool
		// new GUIStyle("button") create the new new GUIStyle base on active GUISkin, which exist only in OnGUI()
		styleCustomButton = new GUIStyle("button")
		{
			fontSize = 10,
			alignment = TextAnchor.UpperLeft,
			wordWrap = true
		};
		layoutOptionsCustomButton = new GUILayoutOption[]
		{
					GUILayout.Width(gridSize),
					GUILayout.Height(gridSize)
		};

		if (_inspected != null)
		{
			_serializedObject.Update();
			if (_showOptions)
				ShowOptions();

			if (_editMode)
			{
				//Show created early the SceneListInEditMode
				list.DoLayoutList();
			}
			else
				ShowSceneListInViewMode();
			_serializedObject.ApplyModifiedProperties();
		}
		else
		{
			throw new NotImplementedException("Resource \"SceneAccessHolder\" is absent");
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
	public void ShowSceneListInViewMode()
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
			int widthCount = gridSize;
			GUILayout.BeginHorizontal();
			for (int i = 0; i < _inspected.sceneList.Count; i++)
			{
				var sceneItem = _inspected.sceneList[i];
				if (!sceneItem.visible)
				{
					continue;
				}

				//GUIContent guiContent = new GUIContent(sceneItem.name);
				if (GUILayout.Button(
					new GUIContent(sceneItem.name),
					styleCustomButton,
					//GUILayout.Width(gridSize),
					//GUILayout.Height(gridSize)
					layoutOptionsCustomButton))
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

	public void CreateSceneListInEditMode()
	{
		//if (_editMode)
		//{
		//	_serializedObject.Update();
		//	list?.DoLayoutList();
		//	_serializedObject.ApplyModifiedProperties();
		//}

		//_serializedObject.Update();
		//_serializedObject = new SerializedObject(_inspected);

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
		//_serializedObject.Update();
		//list.DoLayoutList();
		//_serializedObject.ApplyModifiedProperties();
	}

	/// <summary>
	/// Find all scenes in the project and put them in the list
	/// </summary>
	private void PopulateSceneList(bool IsNewSceneAccessHolder = false)
	{
		//EditorBuildSettingsScene[] currentScenes = EditorBuildSettings.scenes;
		EditorBuildSettingsScene[] filteredScenes = EditorBuildSettings.scenes.Where(ebss => File.Exists(ebss.path)).ToArray();
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
		if (IsNewSceneAccessHolder)
			foreach (SceneAccessHolderSO.SceneInfo sceneInfo in allScene)
					_inspected.sceneList.Add(sceneInfo);
		else
		{
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
		_inspected.showOptions = _showOptions;
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
		_inspected.sceneAccessLayout = _layout;
	}
	private void ToggleEditMode()
	{
		_editMode = !_editMode;
		_inspected.editMode = _editMode;
	}
}
