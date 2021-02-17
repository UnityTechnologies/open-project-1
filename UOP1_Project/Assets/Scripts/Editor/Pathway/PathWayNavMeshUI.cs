using UnityEngine;
using UnityEditorInternal;
using UnityEditor;


public class PathWayNavMeshUI
{
	private Pathway _pathway;
	private PathwayNavMesh _pathwayNavMesh;
	private SerializedProperty _displayProbes;
	private SerializedProperty _toggledNavMeshDisplay;

	private bool DisplayProbes
	{
		get => _displayProbes.boolValue;
		set => _displayProbes.boolValue = value;
	}

	private bool ToggledNavMeshDisplay
	{
		get => _toggledNavMeshDisplay.boolValue;
		set => _toggledNavMeshDisplay.boolValue = value;
	}

	public PathWayNavMeshUI(SerializedObject serializedObject, Pathway pathway)
	{
		_pathway = pathway;
		_displayProbes = serializedObject.FindProperty("DisplayProbes");
		_toggledNavMeshDisplay = serializedObject.FindProperty("ToggledNavMeshDisplay");
		_pathwayNavMesh = new PathwayNavMesh(serializedObject, pathway);
		GeneratePath();
	}

	public void OnInspectorGUI()
	{
		if (!ToggledNavMeshDisplay)
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
				ToggledNavMeshDisplay = false;
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
					ToggledNavMeshDisplay = true;
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
