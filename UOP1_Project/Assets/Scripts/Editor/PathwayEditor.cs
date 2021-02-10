using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof(Pathway))]
public class PathwayEditor : Editor
{
	private ReorderableList _reorderableList;
	private Pathway _pathway;
	private static int _selectedIndex;
	private const string FIELD_LABEL = "Point ";
	private const string TITLE_LABEL = "Waypoints";
	
	protected void OnSceneGUI()
	{
		EditorGUI.BeginChangeCheck();

		Handles.color = _pathway.CubeColor;
		Vector3 vOffset = Vector3.up * _pathway.Size / 2;
		Vector3 cubeDim = Vector3.one * _pathway.Size;

		for (int i = 0; i < _pathway.wayPoints.Count; i++)
		{
			_pathway.wayPoints[i]=Handles.PositionHandle(_pathway.wayPoints[i], Quaternion.identity);
			Handles.Label(_pathway.wayPoints[i] + (_pathway.Size + 2)*Vector3.up, FIELD_LABEL + i);

			if (_selectedIndex != i || _selectedIndex == -1)
			{
				Handles.DrawWireCube(_pathway.wayPoints[i] + vOffset, cubeDim);
			}

			if (i != 0)
			{
				Handles.color = _pathway.LineColor;
				Handles.DrawLine(_pathway.wayPoints[i - 1], _pathway.wayPoints[i]);
				Handles.color = _pathway.CubeColor;
			}
		}

		if (_selectedIndex != -1)
		{
			Handles.color = _pathway.SelectedObjectColor;
			Handles.DrawWireCube(_pathway.wayPoints[_selectedIndex] + vOffset, cubeDim);
		}

		if (_pathway.wayPoints.Count > 2)
		{
			Handles.color = _pathway.LineColor;
			Handles.DrawLine(_pathway.wayPoints[0], _pathway.wayPoints[_pathway.wayPoints.Count-1]);
		}

	}

	private void OnEnable()
	{
		_reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("wayPoints"), true, true, true, true);
		_reorderableList.drawHeaderCallback += DrawHeader;
		_reorderableList.drawElementCallback += DrawElement;
		_reorderableList.onAddCallback += AddItem;
		_reorderableList.onRemoveCallback += RemoveItem;
		_reorderableList.onSelectCallback += SelectItem;
		_selectedIndex = -1;
		_pathway = (Pathway)target;

		if (_pathway.wayPoints == null)
		{
			_pathway.wayPoints = new List<Vector3>();
		}

	}

	private void OnDisable()
	{
		// Make sure we don't get memory leaks etc.
		_reorderableList.drawHeaderCallback -= DrawHeader;
		_reorderableList.drawElementCallback -= DrawElement;
		_reorderableList.onAddCallback -= AddItem;
		_reorderableList.onRemoveCallback -= RemoveItem;
		_reorderableList.onSelectCallback -= SelectItem;
	}

	private void DrawHeader(Rect rect)
	{
		GUI.Label(rect, TITLE_LABEL);
	}

	private void DrawElement(Rect rect, int index, bool active, bool focused)
	{
		var item = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);

		item.vector3Value=EditorGUI.Vector3Field(rect, FIELD_LABEL + index, item.vector3Value);
	}
	
	private void AddItem(ReorderableList list)
	{
		var index=list.index;

		list.serializedProperty.arraySize++;

		if (index > -1 && list.serializedProperty.arraySize > 1)
		{
			for (int i = list.serializedProperty.arraySize - 1; i > index + 1; i--)
			{
				list.serializedProperty.GetArrayElementAtIndex(i).vector3Value = list.serializedProperty.GetArrayElementAtIndex(i - 1).vector3Value;
			}

			list.serializedProperty.GetArrayElementAtIndex(index + 1).vector3Value = new Vector3();
		}
		else
		{
			list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize-1).vector3Value = new Vector3();
		}

	}

	private void RemoveItem(ReorderableList list)
	{
		var index = list.index;

		for (int i = index; i < list.serializedProperty.arraySize - 1; i++)
		{
			list.serializedProperty.GetArrayElementAtIndex(i).vector3Value = list.serializedProperty.GetArrayElementAtIndex(i+1).vector3Value;
		}

		list.serializedProperty.arraySize--;
		_selectedIndex = -1;
	}

	private void SelectItem(ReorderableList list)
	{
		//TODO: add the possibility of placing the box directly with the mouse
		_selectedIndex = list.index;
		SceneView.RepaintAll();
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		serializedObject.Update();
		_reorderableList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();	
	}
}
