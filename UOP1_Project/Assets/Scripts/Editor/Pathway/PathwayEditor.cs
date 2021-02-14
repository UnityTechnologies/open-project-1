using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(Pathway))]
public class PathwayEditor : Editor
{
	private ReorderableList _reorderableList;
	private Pathway _pathway;
	private PathwayHandles _pathwayHandles;


	public void OnSceneGUI()
	{
		_pathwayHandles.DispalyHandles();
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		serializedObject.Update();
		_reorderableList.DoLayoutList();
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
		_reorderableList.onSelectCallback += SelectItem;
		_reorderableList.onChangedCallback += ListModified;
		_pathway = (target as Pathway);
		_pathwayHandles = new PathwayHandles(_pathway);
	}

	private void OnDisable()
	{
		Undo.undoRedoPerformed -= DoUndo;
		_reorderableList.drawHeaderCallback -= DrawHeader;
		_reorderableList.drawElementCallback -= DrawElement;
		_reorderableList.onAddCallback -= AddItem;
		_reorderableList.onRemoveCallback -= RemoveItem;
		_reorderableList.onSelectCallback -= SelectItem;
		_reorderableList.onChangedCallback -= ListModified;
	}

	private void DrawHeader(Rect rect)
	{
		GUI.Label(rect, Pathway.TITLE_LABEL);
	}

	private void DrawElement(Rect rect, int index, bool active, bool focused)
	{
		SerializedProperty item = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
		item.vector3Value = EditorGUI.Vector3Field(rect, Pathway.FIELD_LABEL + index, item.vector3Value);
	}

	private void AddItem(ReorderableList list)
	{
		int index = list.index;

		if (index > -1 && list.serializedProperty.arraySize >= 1)
		{
			list.serializedProperty.InsertArrayElementAtIndex(index + 1);
			Vector3 previous = list.serializedProperty.GetArrayElementAtIndex(index).vector3Value;
			list.serializedProperty.GetArrayElementAtIndex(index + 1).vector3Value = new Vector3(previous.x + 2, previous.y, previous.z + 2);
		}
		else
		{
			list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
			Vector3 previous = _pathway.transform.position;
			list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1).vector3Value = new Vector3(previous.x + 2, previous.y, previous.z + 2);
		}

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

	}

	private void SelectItem(ReorderableList list)
	{
		InternalEditorUtility.RepaintAllViews();
	}

	private void ListModified(ReorderableList list)
	{
		list.serializedProperty.serializedObject.ApplyModifiedProperties();
	}

	private void DoUndo()
	{
		serializedObject.UpdateIfRequiredOrScript();

		if (_reorderableList.index >= _reorderableList.serializedProperty.arraySize)
		{
			_reorderableList.index = _reorderableList.serializedProperty.arraySize - 1;
		}
	}

}
