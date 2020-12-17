using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PointSelection))]
public class PointSelectionDrawer : PropertyDrawer
{
	public enum ErrorCode
	{
		None,
		NullDatabase,
		LocationArrayUnfilled,
		PointArrayUnfilled,
		NullLocationArray,
		EmptyLocationArray,
		NullPointArray,
		EmptyPointArray
	}
	private ErrorCode _errorCode;
	private int _locationIndex, _pointIndex;
	private string[] _locationNames, _pointNames;
	private Rect _locationRect, _pointRect,_locationWarning, _pointWarning;
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//base.OnGUI(position, property, label);
		
		var locationIndex = property.FindPropertyRelative("LocationIndex");
		var pointIndex = property.FindPropertyRelative("PointIndex");
		if (locationIndex != null && pointIndex != null)
		{
			_locationIndex = locationIndex.intValue;
			_pointIndex = pointIndex.intValue;
		}
		else
		{
			Debug.Log("Null Data On Indicies");
		}
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		_locationRect = new Rect(position.x, position.y, 100, position.height);
		_pointRect = new Rect(position.x + 110, position.y, 100, position.height);
		_locationWarning = new Rect(position.x, position.y, 300, position.height);
		_pointWarning = new Rect(position.x + 110, position.y, 300, position.height);
		_errorCode = GetLocationNames();
		switch (_errorCode)
		{
			case ErrorCode.None:
				if (_locationIndex >= _locationNames.Length)
					_locationIndex = (_locationNames.Length > 0) ? _locationNames.Length - 1 : 0;
				_locationIndex = EditorGUI.Popup(_locationRect, _locationIndex, _locationNames);
				_errorCode = GetPointNames(_locationIndex);
				switch (_errorCode)
				{
					case ErrorCode.None:
						if (_pointIndex >= _pointNames.Length)
							_pointIndex = (_pointNames.Length > 0) ?_pointNames.Length - 1 : 0;
						_pointIndex = EditorGUI.Popup(_pointRect, _pointIndex, _pointNames);
						if (locationIndex != null)
						{
							locationIndex.intValue = _locationIndex;
							if (pointIndex != null)
							{
								pointIndex.intValue = _pointIndex;
							}
							property.serializedObject.ApplyModifiedProperties();
						}
						break;
					case ErrorCode.PointArrayUnfilled:
						EditorGUI.HelpBox(_pointWarning, "Selected location's point array is unfilled.", MessageType.Error);
						break;
					case ErrorCode.NullPointArray:
						EditorGUI.HelpBox(_pointWarning, "Selected location has null point array", MessageType.Warning);
						break;
					case ErrorCode.EmptyPointArray:
						EditorGUI.HelpBox(_pointWarning, "Selected location has empty point array", MessageType.Warning);
						break;
				}
				break;
			case ErrorCode.NullDatabase:
				EditorGUI.HelpBox(_locationWarning, "No Location Database. Add one to Resources/Databases folder.", MessageType.Error);
				break;
			case ErrorCode.LocationArrayUnfilled:
				EditorGUI.HelpBox(_locationWarning, "Database location array is unfilled", MessageType.Error);
				break;
			case ErrorCode.NullLocationArray:
				EditorGUI.HelpBox(_locationWarning, "Database location array is Null", MessageType.Error);
				break;
			case ErrorCode.EmptyLocationArray:
				EditorGUI.HelpBox(_locationWarning, "Database location array is Empty", MessageType.Error);
				break;
		}
		EditorGUI.EndProperty();
	}
	private ErrorCode GetLocationNames()
	{
		if (LocationDatabaseSO.Instance == null)
			return ErrorCode.NullDatabase;
		if (LocationDatabaseSO.Instance.Locations == null)
			return ErrorCode.NullLocationArray;
		if (LocationDatabaseSO.Instance.Locations.Length == 0)
			return ErrorCode.EmptyLocationArray;
		_locationNames = new string[LocationDatabaseSO.Instance.Locations.Length];
		for (int i = 0; i < LocationDatabaseSO.Instance.Locations.Length; i++)
		{
			if (LocationDatabaseSO.Instance.Locations[i] == null)
				return ErrorCode.LocationArrayUnfilled;
			_locationNames[i] = LocationDatabaseSO.Instance.Locations[i].SceneName;
		}
		return ErrorCode.None;
	}
	private ErrorCode GetPointNames(int sceneIndex)
	{
		LocationSO scene = LocationDatabaseSO.Instance.Locations[sceneIndex];
		if (scene.Points == null)
			return ErrorCode.NullPointArray;
		if (scene.Points.Length == 0)
			return ErrorCode.EmptyPointArray;
		_pointNames = new string[scene.Points.Length];
		for (int i = 0; i < scene.Points.Length; i++)
		{
			if (scene.Points[i] == null)
				return ErrorCode.PointArrayUnfilled;
			_pointNames[i] = scene.Points[i].PointName;
		}
		return ErrorCode.None;
	}
}
