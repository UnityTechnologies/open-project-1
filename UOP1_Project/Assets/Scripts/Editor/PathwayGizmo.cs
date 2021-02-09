using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof(Pathway))]
public class PathwayGizmo : Editor
{
	private ReorderableList _reorderableList;
	private List<Vector3> _newTargetsPosition;
	private Pathway _pathway;
	static private int _selectedIndex;


	protected void OnSceneGUI()
	{
		EditorGUI.BeginChangeCheck();
		Handles.color = _pathway.CubeColor;
		_newTargetsPosition.Clear();
		for (int i = 0; i < _pathway.wayPoints.Count; i++)
		{
			_newTargetsPosition.Add(Handles.PositionHandle(_pathway.wayPoints[i], Quaternion.identity));
			Handles.DrawWireCube(_pathway.wayPoints[i] + Vector3.up, Vector3.one * _pathway.Size);
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
			Handles.DrawWireCube(_pathway.wayPoints[_selectedIndex] + Vector3.up, Vector3.one * _pathway.Size);
		}

		if (EditorGUI.EndChangeCheck())
		{
			for (int i = 0; i < _pathway.wayPoints.Count; i++)
			{
				_pathway.wayPoints[i] = _newTargetsPosition[i];

			}
		}

	}

	private void OnEnable()
	{
		 _selectedIndex = -1;
		_pathway = (Pathway)target;
		_newTargetsPosition = new List<Vector3>();
		_reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("wayPoints"), true, true, true, true);
		// Add listeners to draw events
		_reorderableList.drawHeaderCallback += DrawHeader;
		_reorderableList.drawElementCallback += DrawElement;
		_reorderableList.onAddCallback += AddItem;
		_reorderableList.onRemoveCallback += RemoveItem;
		_reorderableList.onSelectCallback += SelectItem;
		
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

	/// <summary>
	/// Draws the header of the list
	/// </summary>
	/// <param name="rect"></param>
	private void DrawHeader(Rect rect)
	{
		GUI.Label(rect, "way points");
	}

	/// <summary>
	/// Draws one element of the list (ListItemExample)
	/// </summary>
	/// <param name="rect"></param>
	/// <param name="index"></param>
	/// <param name="active"></param>
	/// <param name="focused"></param>
	private void DrawElement(Rect rect, int index, bool active, bool focused)
	{
		var item = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
		item.vector3Value=EditorGUI.Vector3Field(new Rect(rect.x + 18, rect.y, rect.width - 9, rect.height),"point " + index, item.vector3Value);
		
	}
	
	private void AddItem(ReorderableList list)
	{
		var index = list.serializedProperty.arraySize;
		list.serializedProperty.arraySize++;
		list.index = index;
		list.serializedProperty.GetArrayElementAtIndex(index).vector3Value = new Vector3();
		
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

	private void SelectItem(ReorderableList list) {
		_selectedIndex=list.index;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		serializedObject.Update();
		_reorderableList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
		SceneView.RepaintAll();

	}
}
