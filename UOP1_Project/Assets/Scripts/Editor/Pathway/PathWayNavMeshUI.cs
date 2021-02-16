using UnityEngine;
using UnityEditorInternal;
using UnityEditor;


public class PathWayNavMeshUI
{
	private Pathway _pathway;
	private PathwayNavMesh _pathwayNavMesh;
	private SerializedProperty _displayPolls;
	private SerializedProperty _togglePathDisplay;

	private bool DisplayProbes
	{
		get => _displayPolls.boolValue;
		set => _displayPolls.boolValue = value;
	}

	private bool ToggleNavMeshDisplay
	{
		get => _togglePathDisplay.boolValue;
		set => _togglePathDisplay.boolValue = value;
	}

	public PathWayNavMeshUI(SerializedObject serializedObject, Pathway pathway)
	{
		_pathway = pathway;
		_displayPolls = serializedObject.FindProperty("DisplayProbes");
		_togglePathDisplay = serializedObject.FindProperty("ToggleNavMeshDisplay");
		_pathwayNavMesh = new PathwayNavMesh(serializedObject, pathway);
		GeneratePath();
	}

	public void OnInspectorGUI()
	{
		if (!ToggleNavMeshDisplay)
		{
			if (GUILayout.Button("NavMesh Path"))
			{
				GeneratePath();
			}
		}
		else
		{
			if (GUILayout.Button("Handles Path"))
			{
				ToggleNavMeshDisplay = false;
				DisplayProbes = false;
				InternalEditorUtility.RepaintAllViews();
			}
		}
	}

	public void GeneratePath() {

		DisplayProbes = !_pathwayNavMesh.hasNavMesh();

		if (!DisplayProbes)
		{
			if (_pathway.Waypoints.Count > 1)
			{
				if (_pathwayNavMesh.GenerateNavMeshPath())
				{
					ToggleNavMeshDisplay = true;
					InternalEditorUtility.RepaintAllViews();
				}
			}
		}
		else
		{
			InternalEditorUtility.RepaintAllViews();
		}
	}


	public void PathUpdate()
	{
		if (_pathway.RealTimeEnabled)
		{
			DisplayProbes = !_pathwayNavMesh.hasNavMesh();
			
			if (!DisplayProbes && !GUI.changed)
			{
				_pathwayNavMesh.GenerateNavMeshPath();
			}
		}
	}

	public void RealTime()
	{
		if (_pathway.RealTimeEnabled && GUI.changed)
		{
				GeneratePath();
		}
	}

}
