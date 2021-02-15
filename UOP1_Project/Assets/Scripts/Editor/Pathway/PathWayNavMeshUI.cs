using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using UnityEngine.Events;


public class PathWayNavMeshUI
{
	
	private PathwayNavMesh _pathwayNavMesh;
	private SerializedProperty _displayPolls;
	private SerializedProperty _togglePathDisplay;
	private SerializedProperty _waypoints;

	private bool DisplayPolls
	{
		get => _displayPolls.boolValue;
		set => _displayPolls.boolValue = value;
	}

	private bool TogglePathDisplay
	{
		get => _togglePathDisplay.boolValue;
		set => _togglePathDisplay.boolValue = value;
	}

	public PathWayNavMeshUI(SerializedObject serializedObject)
	{
		_displayPolls = serializedObject.FindProperty("DisplayPolls");
		_togglePathDisplay = serializedObject.FindProperty("TogglePathDisplay");
		_waypoints = serializedObject.FindProperty("Waypoints");
		_pathwayNavMesh = new PathwayNavMesh(serializedObject);
	}

	public void OnInspectorGUI()
	{
		if (!TogglePathDisplay )
		{
			NavMeshPathButton();
		}
		else
		{
			HandlesPathButton();
		}
	}

	private void NavMeshPathButton()
	{
		if ( GUILayout.Button("NavMesh Path"))
		{
			if (_pathwayNavMesh.hasNavMesh())
			{
				if (_waypoints.arraySize > 1)
				{
					if (_pathwayNavMesh.GenerateNavMeshPath())
					{
						TogglePathDisplay = true;
						InternalEditorUtility.RepaintAllViews();
					}
				}
				else
				{
					Debug.LogError("NavMesh need more than one point to calculate the path");
				}
			}
			else
			{
				DisplayPolls = true;
				InternalEditorUtility.RepaintAllViews();
			}
		}
	}

	private void HandlesPathButton()
	{
		if (GUILayout.Button("Handles Path"))
		{
			TogglePathDisplay = false;
			DisplayPolls = false;
			InternalEditorUtility.RepaintAllViews();
		}
	}

	public void OnUpdatePolls() {
		_pathwayNavMesh.hasNavMesh();
	}
	
}
