using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.AI;

[CustomEditor(typeof(Pathway))]

public class PathwayEditor : Editor
{
	private ReorderableList _reorderableList;
	private Pathway _pathway;
	private Vector3 _newPosition;
	private bool _toggled;

	protected void OnSceneGUI()
	{
		DispalyHandles();
	}

	private void DispalyHandles()
	{
		EditorGUI.BeginChangeCheck();
		_newPosition = _pathway.transform.position - _newPosition;

		for (int i = 0; i < _pathway.wayPoints.Length; i++)
		{
			_pathway.wayPoints[i] = Handles.PositionHandle(_pathway.wayPoints[i] + _newPosition, Quaternion.identity);
		}

		_newPosition = _pathway.transform.position;
	}

	private void OnEnable()
	{
		Undo.undoRedoPerformed += DoUndo;
		_reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("wayPoints"), true, true, true, true);
		_reorderableList.drawHeaderCallback += DrawHeader;
		_reorderableList.drawElementCallback += DrawElement;
		_reorderableList.onAddCallback += AddItem;
		_reorderableList.onRemoveCallback += RemoveItem;
		_reorderableList.onSelectCallback += SelectItem;
		_reorderableList.onChangedCallback += ListModified;

		_pathway = (Pathway)target;
		_pathway.SelectedIndex = -1;
		_newPosition = _pathway.transform.position;
		_pathway.Path = new NavMeshPath();
		_toggled = false;
	}

	private void OnDisable()
	{
		// Make sure we don't get memory leaks etc.
		_reorderableList.drawHeaderCallback -= DrawHeader;
		_reorderableList.drawElementCallback -= DrawElement;
		_reorderableList.onAddCallback -= AddItem;
		_reorderableList.onRemoveCallback -= RemoveItem;
		_reorderableList.onSelectCallback -= SelectItem;
		_reorderableList.onChangedCallback -= ListModified;
		Undo.undoRedoPerformed -= DoUndo;
	}

	private void DrawHeader(Rect rect)
	{
		GUI.Label(rect, Pathway.TITLE_LABEL);
	}

	private void DrawElement(Rect rect, int index, bool active, bool focused)
	{
		var item = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
		item.vector3Value = EditorGUI.Vector3Field(rect, Pathway.FIELD_LABEL + index, item.vector3Value);
	}

	private void AddItem(ReorderableList list)
	{
		var index = list.index;
		if (index > -1 && list.serializedProperty.arraySize > 0)
		{
			list.serializedProperty.InsertArrayElementAtIndex(index + 1);
			var previous = list.serializedProperty.GetArrayElementAtIndex(index + 1).vector3Value;
			list.serializedProperty.GetArrayElementAtIndex(index + 1).vector3Value = new Vector3(previous.x + 2, previous.y, previous.z + 2);
		}
		else
		{
			list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
			var previous = _pathway.transform.position;
			list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1).vector3Value = new Vector3(previous.x + 2, previous.y, previous.z + 2);
		}

		list.index++;
		_pathway.SelectedIndex = list.index;
	}

	private void RemoveItem(ReorderableList list)
	{
		var index = list.index;

		list.serializedProperty.DeleteArrayElementAtIndex(index);

		if (list.index == list.serializedProperty.arraySize)
		{
			list.index--;
		}

		_pathway.SelectedIndex = list.index;
	}

	private void SelectItem(ReorderableList list)
	{
		_pathway.SelectedIndex = list.index;
		InternalEditorUtility.RepaintAllViews();
	}

	private void ListModified(ReorderableList list)
	{
		list.serializedProperty.serializedObject.ApplyModifiedProperties();
	}

	private void DoUndo()
	{
		serializedObject.UpdateIfRequiredOrScript();
	}

	void GeneateNavMeshPath(){

		for (int i = 1; i < _pathway.wayPoints.Length; i++)
		{
			NavMesh.CalculatePath(_pathway.wayPoints[i - 1], _pathway.wayPoints[i], NavMesh.AllAreas, _pathway.Path);
		}
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		_reorderableList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();

		if (_toggled == false)
		{
			if (_toggled = GUILayout.Button("NavMesh Path"))
			{
				if (_pathway.wayPoints.Length > 1)
				{
					GeneateNavMeshPath();
				}
				else
					Debug.LogError("Pathway need more than one point to calculate the path");
			}
		}
		else
		{
			if (GUILayout.Button("Handles Path"))
			{
				_toggled = false;
				_pathway.Path.ClearCorners();
			}
		}
	}

}
