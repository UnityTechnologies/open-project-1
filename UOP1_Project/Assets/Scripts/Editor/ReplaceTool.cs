using System.Linq;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Replace tool window,available under Tools window.
/// </summary>
public class ReplaceTool : EditorWindow
{
	// Data store for replace tool.
	ReplaceData data;
	// Data variable wrapped into SerializedObject.
	SerializedObject serializedData;
	// Prefab variable from data object. Using SerializedProperty for integrated Undo.
	SerializedProperty replaceObjectField;

	private void InitDataIfNeeded()
	{
		// If data don't exist, create it.
		if (!data)
		{
			data = ScriptableObject.CreateInstance<ReplaceData>();
			serializedData = null;
		}
		// If data was not wrapped into SerializedObject, wrap it.
		if (serializedData == null)
		{
			serializedData = new SerializedObject(data);
			replaceObjectField = null;
		}
		// If prefab field was not assigned as SerializedProperty, assign it.
		if (replaceObjectField == null)
		{
			replaceObjectField = serializedData.FindProperty("replaceObject");
		}
	}

	// Scroll position for list of selected objects.
	Vector2 selectObjectScrollPosition;

	// Register menu item to open Window.
	[MenuItem("Tools/Replace with Prefab")]
	public static void ShowWindow()
	{
		// Get existing open window or if none, make a new one.
		var window = GetWindow<ReplaceTool>();

		// Open / Show window.
		window.Show();
	}

	private void OnGUI()
	{
		 // Check to have data.
        InitDataIfNeeded();
        // Window title.
        EditorGUILayout.LabelField("Replace Tool", EditorStyles.boldLabel);
        // ReplaceObject field. This object will be used to replace selected game objects.
        EditorGUILayout.PropertyField(replaceObjectField);
        // Space.
        EditorGUILayout.Separator();
        // Title for section with objects to replace.
        EditorGUILayout.LabelField("Selected objects to replace", EditorStyles.boldLabel);
        // Space.
        EditorGUILayout.Separator();
        // Saving number of objects to replace.
        int objectToReplaceCount = data.objectsToReplace != null ? data.objectsToReplace.Length : 0;
        EditorGUILayout.IntField("Object count", objectToReplaceCount);
        // Adding a little indentation.
        EditorGUI.indentLevel++;
        // Printing information when no object is selected on scene.
        if (objectToReplaceCount == 0)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Select object o objects in hierarchy to replace them", EditorStyles.wordWrappedLabel);
        }
        // Scroll view with selected game objects.
        selectObjectScrollPosition = EditorGUILayout.BeginScrollView(selectObjectScrollPosition);
        // Read only list of objects to replace
        GUI.enabled = false;
        if (data && data.objectsToReplace != null)
        {
            foreach (var go in data.objectsToReplace)
            {
                EditorGUILayout.ObjectField(go, typeof(GameObject), true);
            }
        }
        GUI.enabled = true;
        // Closing scroll view.
        EditorGUILayout.EndScrollView();
        // Removing indentation for rest of the window.
        EditorGUI.indentLevel--;
        // Space.
        EditorGUILayout.Separator();
        // Replace button.
        if (GUILayout.Button("Replace"))
        {
	        // Check if replace object is assigned.
	        if (!replaceObjectField.objectReferenceValue)
	        {
		        Debug.LogErrorFormat("[Replace Tool] {0}", "Missing prefab to replace with!");
		        return;
	        }
	        // Check if there are objects to replace.
	        if (data.objectsToReplace.Length == 0)
	        {
		        Debug.LogErrorFormat("[Replace Tool] {0}", "Missing objects to replace!");
		        return;
	        }
	        ReplaceSelectedObjects(data.objectsToReplace, data.replaceObject);

        }
        // Space.
        EditorGUILayout.Separator();
        // Applying any changes on data.
        serializedData.ApplyModifiedProperties();

	}
	private void OnInspectorUpdate()
	{
		// If data was changed, force repaint.
		if (serializedData != null && serializedData.UpdateIfRequiredOrScript())
		{
			this.Repaint();
		}
	}
	private void OnSelectionChange()
	{
		// Check to have data.
		InitDataIfNeeded();
		// Creating filter to gather object only on scene.
		SelectionMode objectFilter = SelectionMode.Unfiltered ^ ~(SelectionMode.Assets | SelectionMode.DeepAssets | SelectionMode.Deep);
		Transform[] selection = Selection.GetTransforms(objectFilter);
		// Converting Transform array into GameObject array.
		data.objectsToReplace = selection.Select(s => s.gameObject).ToArray();
		// Force repaint as update is needed.
		if (serializedData.UpdateIfRequiredOrScript())
		{
			this.Repaint();
		}
	}

	/// <summary>
	/// Replaces game objects with provided replace object.
	/// </summary>
	/// <param name="objectToReplace">Game Objects to replace.</param>
	/// <param name="replaceObject">Replace object.</param>
	private void ReplaceSelectedObjects(GameObject[] objectToReplace, GameObject replaceObject)
	{
		Debug.Log("[Replace Tool] Replace process");
		// Loop through object to replace.
		for (int i = 0; i < objectToReplace.Length; i++)
		{
			var go = objectToReplace[i];
			// Register current game object to Undo action in editor.
			Undo.RegisterCompleteObjectUndo(go, "Saving game object state");
			// Creating replace object as the same position and same parent.
			var inst = Instantiate(replaceObject, go.transform.position, go.transform.rotation, go.transform.parent);
			inst.transform.localScale = go.transform.localScale;
			// Register object creation for Undo action in editor.
			Undo.RegisterCreatedObjectUndo(inst, "Replacement creation.");
			// Changing parent for all children of current game object.
			foreach (Transform child in go.transform)
			{
				// Saving action for Undo action in editor.
				Undo.SetTransformParent(child, inst.transform, "Parent Change");
			}
			// Destroying current game object with save for Undo action in editor.
			Undo.DestroyObjectImmediate(go);
		}
		Debug.LogFormat("[Replace Tool] {0} objects replaced on scene with {1}", objectToReplace.Length, replaceObject.name);
	}
}
