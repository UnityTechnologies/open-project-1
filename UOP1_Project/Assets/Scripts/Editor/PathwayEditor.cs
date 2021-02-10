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
	private static bool _hideGizmos;
	private const string FIELD_LABEL = "Point ";
	private const string TITLE_LABEL = "Waypoints";
	private GUIStyle _style;

	[DrawGizmo(GizmoType.Selected)]
	static void DrawGizmosSelected(Pathway pathway, GizmoType gizmoType)
	{
		if (!_hideGizmos)
		{
			Quaternion lookAt;
			Vector3 vOffset = Vector3.up * pathway.Size / 2;
			Vector3 cubeDim = Vector3.one * pathway.Size;
			Vector3 meshDim = cubeDim / 1.3f;
			Gizmos.color = pathway.CubeColor;

			for (int i = 0; i < pathway.wayPoints.Count; i++)
			{
				if (_selectedIndex != i || _selectedIndex == -1)
				{
					if (i != pathway.wayPoints.Count - 1)
					{
						lookAt = Quaternion.LookRotation(pathway.wayPoints[i + 1] - pathway.wayPoints[i]);
					}
					else
					{
						lookAt = Quaternion.LookRotation(pathway.wayPoints[0] - pathway.wayPoints[i]);
					}

					Gizmos.DrawWireCube(pathway.wayPoints[i] + vOffset, cubeDim);
					Gizmos.DrawMesh(pathway.DrawMesh, pathway.wayPoints[i], lookAt, meshDim);
				}
			}

			if (_selectedIndex != -1)
			{
				if (_selectedIndex != pathway.wayPoints.Count - 1)
				{
					lookAt = Quaternion.LookRotation(pathway.wayPoints[_selectedIndex + 1] - pathway.wayPoints[_selectedIndex]);
				}
				else
				{
					lookAt = Quaternion.LookRotation(pathway.wayPoints[0] - pathway.wayPoints[_selectedIndex]);
				}

				Gizmos.color = pathway.SelectedColor;
				Gizmos.DrawWireCube(pathway.wayPoints[_selectedIndex] + vOffset, cubeDim);
				Gizmos.DrawMesh(pathway.DrawMesh, pathway.wayPoints[_selectedIndex], lookAt, meshDim);
				Gizmos.color = pathway.CubeColor;

			}
		}
	}


	protected void OnSceneGUI()
	{
		EditorGUI.BeginChangeCheck();
		DrawStraightLines();
	}

	private void DrawStraightLines() {

		_style.normal.textColor = _pathway.TextColor;
		Handles.color = _pathway.LineColor;
		
		for (int i = 0; i < _pathway.wayPoints.Count; i++)
		{
			_pathway.wayPoints[i] = Handles.PositionHandle(_pathway.wayPoints[i], Quaternion.identity);
			Handles.Label(_pathway.wayPoints[i] + (_pathway.Size + 2) * Vector3.up, FIELD_LABEL + i, _style);

			if (i != 0)
			{
				Handles.DrawDottedLine(_pathway.wayPoints[i - 1], _pathway.wayPoints[i], 2);
			}
		}

		if (_pathway.wayPoints.Count > 2)
		{
			Handles.DrawDottedLine(_pathway.wayPoints[0], _pathway.wayPoints[_pathway.wayPoints.Count - 1], 2);
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
		_style = new GUIStyle();
		_hideGizmos = false;

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
		_selectedIndex = list.index;
		SceneView.RepaintAll();
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		serializedObject.Update();
		_reorderableList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();

		if (GUILayout.Button("hide gizmos"))
		{
			_hideGizmos=! _hideGizmos;
			SceneView.RepaintAll();
		}
	}

}
