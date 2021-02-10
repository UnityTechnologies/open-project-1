using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof(Pathway))]
public class PathwayEditor : Editor
{
	private ReorderableList _reorderableList;
	private Pathway _pathway;
	private const string FIELD_LABEL = "Point ";
	private const string TITLE_LABEL = "Waypoints";
	
	protected void OnSceneGUI()
	{
		EditorGUI.BeginChangeCheck();
		DispalyHandles();
	}

	private void DispalyHandles()
	{
		for (int i = 0; i < _pathway.wayPoints.Count; i++)
		{
			_pathway.wayPoints[i] = Handles.PositionHandle(_pathway.wayPoints[i], Quaternion.identity);
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
		_pathway = (Pathway)target;
		_pathway.SelectedIndex = -1;

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
		_pathway.SelectedIndex = -1;
	}

	private void SelectItem(ReorderableList list)
	{
		_pathway.SelectedIndex = list.index;
		SceneView.RepaintAll();
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		serializedObject.Update();
		_reorderableList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();

		if (GUILayout.Button("button"))
		{
			
		}
	}

}
