using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CustomEditor(typeof(PathwayConfigSO))]
public class PathwayEditor : Editor
{
	private ReorderableList _reorderableList;
	private PathwayConfigSO _pathway;
	private PathwayHandles _pathwayHandles;
	private PathWayNavMeshUI _pathWayNavMeshUI;
	private enum LIST_MODIFICATION { ADD, SUPP, DRAG, OTHER };
	private LIST_MODIFICATION _currentListModification;
	private int _indexCurrentModification;

	public void OnSceneGUI(SceneView sceneView)
	{
		int index = _pathwayHandles.DisplayHandles();
		_pathWayNavMeshUI.RealTime(index);
		PathwayGizmos.DrawGizmosSelected(_pathway);
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		serializedObject.Update();
		_reorderableList.DoLayoutList();
		_pathWayNavMeshUI.OnInspectorGUI();
		serializedObject.ApplyModifiedProperties();
	}

	private void OnEnable()
	{
		Undo.undoRedoPerformed += DoUndo;
		_reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("Waypoints"), true, true, true, true);
		_reorderableList.drawHeaderCallback += DrawHeader;
		_reorderableList.drawElementCallback += DrawElement;
		_reorderableList.onAddCallback += AddItem;
		_reorderableList.onRemoveCallback += RemoveItem;
		_reorderableList.onChangedCallback += ListModified;
		_reorderableList.onMouseDragCallback += DragItem;
		_pathway = (target as PathwayConfigSO);
		_pathWayNavMeshUI = new PathWayNavMeshUI(_pathway);
		_pathwayHandles = new PathwayHandles(_pathway);
		_currentListModification = LIST_MODIFICATION.OTHER;
		SceneView.duringSceneGui += this.OnSceneGUI;
	}

	private void OnDisable()
	{
		Undo.undoRedoPerformed -= DoUndo;
		_reorderableList.drawHeaderCallback -= DrawHeader;
		_reorderableList.drawElementCallback -= DrawElement;
		_reorderableList.onAddCallback -= AddItem;
		_reorderableList.onRemoveCallback -= RemoveItem;
		_reorderableList.onChangedCallback -= ListModified;
		_reorderableList.onMouseDragCallback -= DragItem;
		SceneView.duringSceneGui -= this.OnSceneGUI;
	}

	private void DrawHeader(Rect rect)
	{
		GUI.Label(rect, PathwayConfigSO.TITLE_LABEL);
	}

	private void DrawElement(Rect rect, int index, bool active, bool focused)
	{
		SerializedProperty item = _reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("waypoint");
		item.vector3Value = EditorGUI.Vector3Field(rect, PathwayConfigSO.FIELD_LABEL + index, item.vector3Value);
	}

	private void AddItem(ReorderableList list)
	{
		int index = list.index;

		if (index > -1 && list.serializedProperty.arraySize >= 1)
		{
			list.serializedProperty.InsertArrayElementAtIndex(index + 1);
			Vector3 previous = list.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("waypoint").vector3Value;
			list.serializedProperty.GetArrayElementAtIndex(index + 1).FindPropertyRelative("waypoint").vector3Value = new Vector3(previous.x + 2, previous.y, previous.z + 2);
			_indexCurrentModification = index + 1;
		}
		else
		{
			list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
			Vector3 previous = Vector3.zero;
			list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1).FindPropertyRelative("waypoint").vector3Value = new Vector3(previous.x + 2, previous.y, previous.z + 2);
			_indexCurrentModification = list.serializedProperty.arraySize - 1;
		}
		_currentListModification = LIST_MODIFICATION.ADD;
		list.index++;
	}

	private void RemoveItem(ReorderableList list)
	{
		int index = list.index;

		list.serializedProperty.DeleteArrayElementAtIndex(index);

		if (list.index == list.serializedProperty.arraySize)
		{
			list.index--;
		}
		_indexCurrentModification = index - 1;
		_currentListModification = LIST_MODIFICATION.SUPP;
	}

	private void DragItem(ReorderableList list)
	{
		_indexCurrentModification = list.index;
		_currentListModification = LIST_MODIFICATION.DRAG;
	}

	private void ListModified(ReorderableList list)
	{
		list.serializedProperty.serializedObject.ApplyModifiedProperties();

		switch (_currentListModification)
		{
			case LIST_MODIFICATION.ADD:
				_pathWayNavMeshUI.UpdatePathAt(_indexCurrentModification);
				break;

			case LIST_MODIFICATION.SUPP:
				if (list.serializedProperty.arraySize > 1)
				{
					_pathWayNavMeshUI.UpdatePathAt((list.serializedProperty.arraySize + _indexCurrentModification) % list.serializedProperty.arraySize);
				}
				break;
			case LIST_MODIFICATION.DRAG:
				_pathWayNavMeshUI.UpdatePathAt(list.index);
				_pathWayNavMeshUI.UpdatePathAt(_indexCurrentModification);
				break;
			default:
				break;
		}
		_currentListModification = LIST_MODIFICATION.OTHER;
	}

	private void DoUndo()
	{
		serializedObject.UpdateIfRequiredOrScript();

		if (_reorderableList.index >= _reorderableList.serializedProperty.arraySize)
		{
			_reorderableList.index = _reorderableList.serializedProperty.arraySize - 1;
		}
		_pathWayNavMeshUI.GeneratePath();
	}

}
