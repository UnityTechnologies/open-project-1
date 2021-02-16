using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(PathwayConfigSO))]
public class PathwayEditor : Editor
{
	private ReorderableList _reorderableList;
	private PathwayConfigSO _pathway;

	public void OnSceneGUI(SceneView sceneView)
	{
		PathwayGizmos.DrawHandlesPath(_pathway);
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
		_reorderableList.onChangedCallback += ListModified;
		_pathway = (target as PathwayConfigSO);
		if (CheckNavMeshExistence())
		{
			SceneView.duringSceneGui += this.OnSceneGUI;
		}
		else
		{
			Debug.LogWarning("Pathway edition not available on this scene. NavMesh baked data missing.");
		}
	}

	private void OnDisable()
	{
		Undo.undoRedoPerformed -= DoUndo;
		_reorderableList.drawHeaderCallback -= DrawHeader;
		_reorderableList.drawElementCallback -= DrawElement;
		_reorderableList.onAddCallback -= AddItem;
		_reorderableList.onRemoveCallback -= RemoveItem;
		_reorderableList.onChangedCallback -= ListModified;
		SceneView.duringSceneGui -= this.OnSceneGUI;
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

		if (index < 0)
		{
			index = list.serializedProperty.arraySize - 1;
		}

		if (list.serializedProperty.arraySize >= 1)
		{
			Vector3 previous = (list.serializedProperty.GetArrayElementAtIndex(index).vector3Value + list.serializedProperty.GetArrayElementAtIndex((index + 1) % list.serializedProperty.arraySize).vector3Value) / 2;
			list.serializedProperty.InsertArrayElementAtIndex(index + 1);
			list.serializedProperty.GetArrayElementAtIndex(index + 1).vector3Value = new Vector3(previous.x, previous.y, previous.z);
		}
		else
		{
			Vector3 previous = Vector3.zero;
			list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
			list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1).vector3Value = new Vector3(previous.x, previous.y, previous.z);
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

	private bool CheckNavMeshExistence()
	{
		return NavMesh.SamplePosition(Vector3.zero, out NavMeshHit hit, 1000.0f, NavMesh.AllAreas);
	}

}
