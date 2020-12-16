using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LocationSelection))]
public class LocationSelectionDrawer : PropertyDrawer
{
	public enum ErrorCode
	{
		None,
		NullDatabase,
		SceneArrayUnfilled,
		LocationArrayUnfilled,
		NullSceneArray,
		EmptySceneArray,
		NullLocationArray,
		EmptyLocationArray
	}
	private ErrorCode _errorCode;
	private int _sceneIndex, _locationIndex;
	private string[] _sceneNames, _locationNames;
	private Rect _sceneRect, _locationRect,_sceneWarning, _locationWarning;
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//base.OnGUI(position, property, label);
		
		var sceneIndex = property.FindPropertyRelative("SceneIndex");
		var locationIndex = property.FindPropertyRelative("LocationIndex");
		if (sceneIndex != null && locationIndex != null)
		{
			_sceneIndex = sceneIndex.intValue;
			_locationIndex = locationIndex.intValue;
		}
		else
		{
			Debug.Log("Null Data On Indicies");
		}
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		_sceneRect = new Rect(position.x, position.y, 100, position.height);
		_locationRect = new Rect(position.x + 110, position.y, 100, position.height);
		_sceneWarning = new Rect(position.x, position.y, 300, position.height);
		_locationWarning = new Rect(position.x + 110, position.y, 300, position.height);
		_errorCode = GetSceneNames();
		switch (_errorCode)
		{
			case ErrorCode.None:
				if (_sceneIndex >= _sceneNames.Length)
					_sceneIndex = (_sceneNames.Length > 0) ? _sceneNames.Length - 1 : 0;
				_sceneIndex = EditorGUI.Popup(_sceneRect, _sceneIndex, _sceneNames);
				_errorCode = GetLocationNames(_sceneIndex);
				switch (_errorCode)
				{
					case ErrorCode.None:
						if (_locationIndex >= _locationNames.Length)
							_locationIndex = (_locationNames.Length > 0) ?_locationNames.Length - 1 : 0;
						_locationIndex = EditorGUI.Popup(_locationRect, _locationIndex, _locationNames);
						if (sceneIndex != null)
						{
							sceneIndex.intValue = _sceneIndex;
							if (locationIndex != null)
							{
								locationIndex.intValue = _locationIndex;
							}
							property.serializedObject.ApplyModifiedProperties();
						}
						break;
					case ErrorCode.LocationArrayUnfilled:
						EditorGUI.HelpBox(_locationWarning, "Selected scene's location array is unfilled.", MessageType.Error);
						break;
					case ErrorCode.NullLocationArray:
						EditorGUI.HelpBox(_locationWarning, "Selected scene has null location array", MessageType.Warning);
						break;
					case ErrorCode.EmptyLocationArray:
						EditorGUI.HelpBox(_locationWarning, "Selected scene has empty location array", MessageType.Warning);
						break;
				}
				break;
			case ErrorCode.NullDatabase:
				EditorGUI.HelpBox(_sceneWarning, "No Scene Database. Add one to Resources/Databases folder.", MessageType.Error);
				break;
			case ErrorCode.SceneArrayUnfilled:
				EditorGUI.HelpBox(_sceneWarning, "Database scene array is unfilled", MessageType.Error);
				break;
			case ErrorCode.NullSceneArray:
				EditorGUI.HelpBox(_sceneWarning, "Database scene array is Null", MessageType.Error);
				break;
			case ErrorCode.EmptySceneArray:
				EditorGUI.HelpBox(_sceneWarning, "Database scene array is Empty", MessageType.Error);
				break;
		}
		EditorGUI.EndProperty();
	}
	private ErrorCode GetSceneNames()
	{
		if (SceneDatabaseSO.Instance == null)
			return ErrorCode.NullDatabase;
		if (SceneDatabaseSO.Instance.Scenes == null)
			return ErrorCode.NullSceneArray;
		if (SceneDatabaseSO.Instance.Scenes.Length == 0)
			return ErrorCode.EmptySceneArray;
		_sceneNames = new string[SceneDatabaseSO.Instance.Scenes.Length];
		for (int i = 0; i < SceneDatabaseSO.Instance.Scenes.Length; i++)
		{
			if (SceneDatabaseSO.Instance.Scenes[i] == null)
				return ErrorCode.SceneArrayUnfilled;
			_sceneNames[i] = SceneDatabaseSO.Instance.Scenes[i].SceneName;
		}
		return ErrorCode.None;
	}
	private ErrorCode GetLocationNames(int sceneIndex)
	{
		SceneSO scene = SceneDatabaseSO.Instance.Scenes[sceneIndex];
		if (scene.Locations == null)
			return ErrorCode.NullLocationArray;
		if (scene.Locations.Length == 0)
			return ErrorCode.EmptyLocationArray;
		_locationNames = new string[scene.Locations.Length];
		for (int i = 0; i < scene.Locations.Length; i++)
		{
			if (scene.Locations[i] == null)
				return ErrorCode.LocationArrayUnfilled;
			_locationNames[i] = scene.Locations[i].LocationName;
		}
		return ErrorCode.None;
	}
}
